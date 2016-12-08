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

	private float _TimeToNextRule = Mathf.Infinity;
	private float _TimeLeftOnRule = Mathf.Infinity;

	public Gesture() {
		RuleIndex = -1;
		Rules = new GestureRule[2];
	}

	public void GestureUpdate(HeadTracker head, HandsTracker hands) {

		if (Completed || Disabled) {
			return;
		}
		else if (RuleIndex >= Rules.Length - 1) {
			_DelayRemaining -= Time.deltaTime;
		}

		if (Input.GetKeyDown(ForceCompleteKey)) {
			RuleIndex = Rules.Length;
			return;
		}

		if (CheckRule(head, hands, Rules[RuleIndex+1])) {
			RuleIndex++;
			//Debug.Log("Completed rule: " + RuleIndex + " of " + Name);
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
				//Debug.Log(Name + " -- timed out on rule " + RuleIndex);
				Reset();
			}
		}
		else {
			_TimeToNextRule -= Time.deltaTime;
			if (_TimeToNextRule <= 0f) {
				//Debug.Log(Name + " -- timed out getting to rule " + RuleIndex);
				Reset();
			}
		}
	}

	public void Reset() {
		RuleIndex = -1;
		_TimeLeftOnRule = Mathf.Infinity;
		_TimeToNextRule = Mathf.Infinity;
	    Debug.Log("reseting " + Name);
	}

	private bool CheckRule(HeadTracker head, HandsTracker hands, GestureRule rule) {
		if (rule.RequireHeadState) {
			if (!head.HeadStateList.Contains(rule.HeadState)) {
				return false;
			}
		}	


		var leftHandInLeftZone = false;
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
	        	if (rule.LeftHandAngles.Contains(leftAngle)) {
	        		leftHandInLeftZone = true;
	        	}
	        	/* if the leftHand is in neither zone, then the rule is false */
	        	else if (!rule.RightHandAngles.Contains(leftAngle)) {
	  				return false;
	  			}

	  			/* check if right hand is in the zone the left hand is not */
	  			if (leftHandInLeftZone && !rule.RightHandAngles.Contains(rightAngle)) {
	  				return false;	
	  			}
	  			else if (!leftHandInLeftZone && !rule.LeftHandAngles.Contains(rightAngle)) {
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
				if (!rule.RequireHandAngles) {
					if (rule.LeftHandReach.Contains(leftReach)) {
						leftHandInLeftZone = true;
					}
				}
				else if (leftHandInLeftZone && !rule.LeftHandReach.Contains(leftReach)) {
					return false;
				}

				if (!leftHandInLeftZone && !rule.RightHandReach.Contains(leftReach)) {
					return false;
				}

				if (leftHandInLeftZone && !rule.RightHandReach.Contains(rightReach)) {
					return false;
				}
				else if (!leftHandInLeftZone && !rule.LeftHandReach.Contains(rightReach)) {
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
				if (!rule.RequireHandAngles && !rule.RequireHandReach) {
					if (rule.LeftHandWaving && leftWaving) {
						leftHandInLeftZone = true;
					}
				}

                /* if the left hand is in the left zone, compare the required
                 * waving state against the appropriate hand */
                if (leftHandInLeftZone) {
                    if (rule.LeftHandWaving != leftWaving) {
                        return false;
                    }
                    if (rule.RightHandWaving != rightWaving) {
                        return false;
                    }
                }
                /* if the left hand is not in the left zone, compare the required
                 * waving state against the opposite hand */
                if (!leftHandInLeftZone) {
                    if (rule.LeftHandWaving != rightWaving) {
                        return false;
                    }
                    if (rule.RightHandWaving != leftWaving) {
                        return false;
                    }
                }

				/* perform checks for the left hand */
				/* if LEFT HAND is acting in the LEFT ZONE:
				* check if a LEFT ZONE wave is required and if the LEFT HAND is doing it */
				//if (leftHandInLeftZone && rule.LeftHandWaving && !leftWaving) {
				//	return false;
				//}
				/* if LEFT HAND is acting in the RIGHT ZONE:
				* check if a RIGHT ZONE wave required and if the LEFT HAND is doing it */
				//else if (!leftHandInLeftZone && rule.RightHandWaving && !leftWaving) {
				//	return false;
				//}

				/* perform checks for the right hand */
				/* if LEFT HAND is acting in the LEFT ZONE:
				* check if a RIGHT ZONE wave is required, and if the RIGHT HAND is doing it */
				//if (leftHandInLeftZone && rule.RightHandWaving && !rightWaving) {
				//	return false;
				//}
				/* if the LEFT HAND is acting in the RIGHT ZONE:
				* check if a LEFT ZONE wave is required, and if the RIGHT HAND is doing it */
				//else if (!leftHandInLeftZone && rule.LeftHandWaving && !rightWaving) {
				//	return false;
				//}
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