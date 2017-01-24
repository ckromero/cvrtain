using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Gesture {

	public string Name;
	public KeyCode ForceCompleteKey;
	public GestureRule[] Rules;
	public float EvaluationDelay = 0f;
	private float _DelayRemaining = 0f;

	public bool Disabled = false;

	public int RuleIndex { get; private set; }
	public bool Completed {
		get {
			return (RuleIndex >= Rules.Length - 1 && _DelayRemaining <= 0f);
		}
	}
    public bool DelayedComplete
    {
        get
        {
            return (RuleIndex >= Rules.Length - 1);
        }
    }

	private float _TimeToNextRule = Mathf.Infinity;
	private float _TimeLeftOnRule = Mathf.Infinity;

	/* in the event that the Gesture is designed to be hand agnostic (ie, the 
	* EitherHand bool in one of the GestureRules is true) then these are used to
	* track which hand should be assoicated with which part of the rules. Should
	* left hand check against the left or the right part of the rule, and vica versa
	* for the right hand */

	// Reveals whether the binding of a hand to a side occured yet?
	private bool _LeftHandZoneSet = false;
	// Is the left hand bound to the left zone? If false, then it's bound to the right
	private bool _LeftHandInLeftZone = false;

	private Vector3 LastHeadPosition;

	public Gesture() {
		RuleIndex = -1;
		Rules = new GestureRule[2];
	}

	public void GestureUpdate(HeadTracker head, HandsTracker hands) {
		if (Completed || Disabled) {
			return;
		}

		if (Input.GetKeyDown(ForceCompleteKey)) {
			RuleIndex = Rules.Length;
			return;
		}

		if (RuleIndex >= Rules.Length - 1) {
			_DelayRemaining -= Time.deltaTime;
            return;
		}

		if (CheckRule(head, hands, Rules[RuleIndex+1])) {
			RuleIndex++;
            LastHeadPosition = head.HeadTransform.localPosition;
            if (RuleIndex >= Rules.Length - 1) {
				_DelayRemaining = EvaluationDelay;
			}
			if (!Completed) {
				var rule = Rules[RuleIndex];
				var time = (rule.HasMaximumDuration) ? rule.MaxDuration : Mathf.Infinity;
				_TimeLeftOnRule = time;
				time = (rule.HasTimeLimit) ? rule.TimeLimit : Mathf.Infinity;
				_TimeToNextRule = time;
			}
		}
		else if (RuleIndex >= 0 && CheckRule(head, hands, Rules[RuleIndex])) {
			_TimeLeftOnRule -= Time.deltaTime;
			if (_TimeLeftOnRule <= 0f) {
				Reset();
			}
		}
		else {
			_TimeToNextRule -= Time.deltaTime;
			if (_TimeToNextRule <= 0f) {
				Reset();
			}
		}
	}

	public void Reset() {
		RuleIndex = -1;
		_TimeLeftOnRule = Mathf.Infinity;
		_TimeToNextRule = Mathf.Infinity;
        _LeftHandZoneSet = false;
        Debug.Log("reseting " + Name);
    }

    /* check against all the different possible values for the current rule against
    * the current value contained in the HeadTracker and HandsTracker. */
    private bool CheckRule(HeadTracker head, HandsTracker hands, GestureRule rule) {
		if (rule.RequireHeadState) {
            if (head.HeadState != rule.HeadState) {
				return false;
			}
		}	

        if (rule.RequireHandAngles) {
        	var leftAngle = hands.LeftHandAngle;
        	var rightAngle = hands.RightHandAngle;
        	if (!rule.EitherHand) {
        		if (!rule.LeftHandAngles.Contains(leftAngle)) {
	        		return false;
	        	}
	        	if (!rule.RightHandAngles.Contains(rightAngle)) {
	        		return false;
	        	}
	        }
	        else {
	        	if (!_LeftHandZoneSet) {
		        	if (rule.LeftHandAngles.Contains(leftAngle)) {
		        		_LeftHandInLeftZone = true;
		        	}
                    else if (rule.LeftHandAngles.Contains(rightAngle)) {
                        _LeftHandInLeftZone = false;
                    }
                    else {
                    	return false;
                    }
                    _LeftHandZoneSet = true;
	        	}
		        if (_LeftHandInLeftZone && !rule.LeftHandAngles.Contains(leftAngle)) {
		       		return false;
		       	}
		       	/* if the leftHand is in neither zone, then the rule is false */
		       	else if (!_LeftHandInLeftZone && !rule.RightHandAngles.Contains(leftAngle)) {
	  				return false;
	  			}

	  			/* check if right hand is in the zone the left hand is not */
	  			if (_LeftHandInLeftZone && !rule.RightHandAngles.Contains(rightAngle)) {
	  				return false;	
	  			}
	  			else if (!_LeftHandInLeftZone && !rule.LeftHandAngles.Contains(rightAngle)) {
	  				return false;
	  			}
	        }
        }

		if (rule.RequireHandReach) {
			var leftReach = hands.LeftHandRing;
			var rightReach = hands.RightHandRing;
			if (!rule.EitherHand) {
				if (!rule.LeftHandReach.Contains(leftReach)) {
					return false;
				}
				if (!rule.RightHandReach.Contains(rightReach)) {
					return false;
				}
			}
			else {
				if (!_LeftHandZoneSet) {
					if (rule.LeftHandReach.Contains(leftReach)) {
						_LeftHandInLeftZone = true;
					}
                    else if (rule.LeftHandReach.Contains(rightReach)) {
                        _LeftHandInLeftZone = false;
                    }
                    else {
                    	return false;
                    }
					_LeftHandZoneSet = true;
				}
				if (_LeftHandInLeftZone && !rule.LeftHandReach.Contains(leftReach)) {
					return false;
				}
				else if (!_LeftHandInLeftZone && !rule.RightHandReach.Contains(leftReach)) {
					return false;
				}

				if (_LeftHandInLeftZone && !rule.RightHandReach.Contains(rightReach)) {
					return false;
				}
				else if (!_LeftHandInLeftZone && !rule.LeftHandReach.Contains(rightReach)) {
					return false;
				}
			}
		}

		if (rule.Waving) {
			if (!hands.Waving) {
				return false;
			}
			var leftWaving = hands.LeftHandWaving;
			var rightWaving = hands.RightHandWaving;
			if (!rule.EitherHand) {
				if (rule.LeftHandWaving != leftWaving) {
					return false;
				}
				if (rule.RightHandWaving != rightWaving) {
					return false;
				}
			}
			else {
				/* if neither angles nor reach were required, then check to
				* determine which zone the left hand is acting in and set it */
				if (!_LeftHandZoneSet) {
					if (rule.LeftHandWaving && leftWaving) {
						_LeftHandInLeftZone = true;
					}
                    else if (rule.LeftHandWaving && rightWaving) {
                        _LeftHandInLeftZone = false;
                    }
                    else {
                    	return false;
                    }
                    _LeftHandZoneSet = true;
				}

                /* if the left hand is in the left zone, compare the required
                 * waving state against the appropriate hand */
                if (_LeftHandInLeftZone) {
                    if (rule.LeftHandWaving != leftWaving) {
                        return false;
                    }
                    if (rule.RightHandWaving != rightWaving) {
                        return false;
                    }
                }
                /* if the left hand is not in the left zone, compare the required
                 * waving state against the opposite hand */
                if (!_LeftHandInLeftZone) {
                    if (rule.LeftHandWaving != rightWaving) {
                        return false;
                    }
                    if (rule.RightHandWaving != leftWaving) {
                        return false;
                    }
                }
			}
		}

		if (rule.RequireHandDistance) {
			var leftLocal = hands.LeftHand.localPosition;
			var rightLocal = hands.RightHand.localPosition;
			var distance = Vector3.Distance(leftLocal, rightLocal);

			if (!rule.DistanceBetweenHands.Contains(distance)) {
				return false;
			}
		}

		if (rule.RequireHandToHeadDistance) {
			var leftLocal = hands.LeftHand.localPosition;
			var rightLocal = hands.RightHand.localPosition;
			var headLocal = head.HeadTransform.localPosition;
			var leftDistance = Vector3.Distance(leftLocal, headLocal);
			var rightDistance = Vector3.Distance(rightLocal, headLocal);

			if (!rule.EitherHand) {
				if (!rule.LeftHandHeadDistance.Contains(leftDistance)) {
					return false;
				}
				if (!rule.RightHandHeadDistance.Contains(rightDistance)) {
					return false;
				}
			}
			else {
				if (!_LeftHandZoneSet) {
					if (rule.LeftHandHeadDistance.Contains(leftDistance)) {
						_LeftHandInLeftZone = true;
					}
                    else if (rule.LeftHandHeadDistance.Contains(rightDistance)) {
                        _LeftHandInLeftZone = false;
                    }
                    else {
                    	return false;
                    }
					_LeftHandZoneSet = true;
				}
				if (_LeftHandInLeftZone && !rule.LeftHandHeadDistance.Contains(leftDistance)) {
					return false;
				}
				else if (!_LeftHandInLeftZone && !rule.RightHandHeadDistance.Contains(leftDistance)) {
					return false;
				}

				if (_LeftHandInLeftZone && !rule.RightHandHeadDistance.Contains(rightDistance)) {
					return false;
				}
				else if (!_LeftHandInLeftZone && !rule.LeftHandHeadDistance.Contains(rightDistance)) {
					return false;
				}
			}
		}

		if (rule.RequireHeadMovement) {
			var headPosition = head.HeadTransform.localPosition;
			var distance = Vector3.Distance(headPosition, LastHeadPosition);

			if (!rule.HeadMovement.Contains(distance)) {
				return false;	
			}
		}

		return true;
	}
}

[System.Serializable]
public class GestureRule {
	public bool RequireHeadState;
	public HeadState HeadState;
	public bool RequireHandAngles;
	public bool EitherHand;
	public AngleRange LeftHandAngles;
	public AngleRange RightHandAngles;
	public bool RequireHandReach;
	public Range LeftHandReach;
	public Range RightHandReach;
	public bool Waving;
	public bool LeftHandWaving;
	public bool RightHandWaving;
	public bool RequireHandDistance;
	public Range DistanceBetweenHands;
	public bool RequireHandToHeadDistance;
	public Range LeftHandHeadDistance;
	public Range RightHandHeadDistance;
	public bool RequireHeadMovement;
	public Range HeadMovement;

	public bool HasMaximumDuration;
	public float MaxDuration;
	public bool HasTimeLimit;
	public float TimeLimit;

	public GestureRule() {}
}	

[System.Serializable]
public class Range {
	public float Max;
	public float Min;

	public Range(float min, float max) {
		Max = max;
		Min = min;
	}

	public virtual bool Contains(float number) {
		return Min <= number && number <= Max;
	}
}

[System.Serializable]
public class AngleRange : Range {

	public AngleRange(float min, float max) : base(min, max) {}

	public override bool Contains(float number) {
		if (number < Min) {
			number += 360;
		}
		else if (number > Max) {
			number -= 360;
		}
		return Min <= number && number <= Max;	
	}
}