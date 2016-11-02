using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Gesture {

	public string Name;
	public GestureRule[] Rules;

	public int RuleIndex { get; private set; }
	public bool Completed {
		get {
			return RuleIndex >= Rules.Length - 1;
		}
	}

	private float _TimeToNextRule = Mathf.Infinity;
	private float _TimeLeftOnRule = Mathf.Infinity;

	public Gesture() {
		RuleIndex = -1;
		Rules = new GestureRule[2];
	}

	public void GestureUpdate(HeadTracker head, HandsTracker hands) {

		if (Completed) {
			return;
		}

		if (CheckRule(head, hands, Rules[RuleIndex+1])) {
			RuleIndex++;
			Debug.Log("Completed rule: " + RuleIndex + " of " + Name);
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
				Debug.Log(Name + " -- timed out on rule " + RuleIndex);
				Reset();
			}
		}
		else {
			_TimeToNextRule -= Time.deltaTime;
			if (_TimeToNextRule <= 0f) {
				Debug.Log(Name + " -- timed out getting to rule " + RuleIndex);
				Reset();
			}
		}
	}

	public void Reset() {
		RuleIndex = -1;
		_TimeLeftOnRule = Mathf.Infinity;
		_TimeToNextRule = Mathf.Infinity;
	}

	private bool CheckRule(HeadTracker head, HandsTracker hands, GestureRule rule) {
		// var ruleComplete = true;
		if (rule.RequireHeadState) {
			if (!head.HeadStateList.Contains(rule.HeadState)) {
				// ruleComplete = false;
				return false;
			}
		}	

        if (rule.RequireHandAngles) {
        	var leftZone = hands.LeftHandZone;
        	var rightZone = hands.RightHandZone;
        	var leftAngle = hands.LeftHandAngle;
        	var rightAngle = hands.RightHandAngle;
        	if (!rule.EitherHandAngle) {
        		if (!rule.LeftHandAngles.Contains(leftAngle)) {
	        	// if (rule.LeftHandAngles.x < leftZone ||
	        	// 		leftZone < rule.LeftHandAngles.y) {
	        		// ruleComplete = false;
	        		return false;
	        	}
	        	if (!rule.RightHandAngles.Contains(rightAngle)) {
	        	// if (rule.RightHandAngles.x > rightZone ||
	        	// 		rightZone > rule.RightHandAngles.y) {
	        		// ruleComplete = false;
	        		return false;
	        	}
	        }
	        else {
	        	var inLeftZone = false;
	        	if (rule.LeftHandAngles.Contains(leftAngle)) {
	        	// if (rule.LeftHandAngles.x > leftZone &&
	        	// 	leftZone > rule.LeftHandAngles.y) {
	        		inLeftZone = true;
	        	}
	        	/* if the leftHand is in neither zone, then the rule is false */
	        	else if (!rule.RightHandAngles.Contains(leftAngle)) {
	  			// else if (rule.RightHandAngles.x < leftZone &&
	  			// 	leftZone < rule.RightHandAngles.y) {
	  				return false;
	  			}

	  			/* check if right hand is in the zone the left hand is not */
	  			if (inLeftZone && !rule.RightHandAngles.Contains(rightAngle)) {
	  			// if (inLeftZone &&
	  			// 	rule.RightHandAngles.x > rightZone &&
	  			// 	rightZone > rule.RightHandAngles.y) {
	  				return false;	
	  			}
	  			else if (!inLeftZone && !rule.LeftHandAngles.Contains(rightAngle)) {
	  			// else if (!inLeftZone &&
	  						// rule.LeftHandAngles.x < rightZone &&
	  						// rightZone < rule.LeftHandAngles.y) {
	  				return false;
	  			}
	        }
        }

		if (rule.RequireHandReach) {
			var leftReach = hands.LeftHandRing;
			var rightReach = hands.RightHandRing;
			if (!rule.EitherHandReach) {
				if (!rule.LeftHandReach.Contains(leftReach)) {
				// if (rule.LeftHandReach.x > leftReach ||
				// 	rule.LeftHandReach.y < leftReach) {
					// ruleComplete = false;
					return false;
				}
				if (!rule.RightHandReach.Contains(rightReach)) {
				// if (rule.RightHandReach.x > rightReach ||
				// 	rule.RightHandReach.y < rightReach) {
					// ruleComplete = false;
					return false;
				}
			}
			else {
				var inLeftZone = false;
				if (rule.LeftHandReach.Contains(leftReach)) {
				// if (rule.LeftHandReach.x < leftReach && 
				// 	leftReach < rule.LeftHandReach.y) {
					inLeftZone = true;
				}
				else if (!rule.RightHandReach.Contains(leftReach)) {
				// else if (rule.RightHandReach.x > leftReach &&
				// 			leftReach > rule.RightHandReach.y) {
					return false;
				}

				if (inLeftZone && !rule.RightHandReach.Contains(rightReach)) {
				// if (inLeftZone &&
				// 	rule.RightHandReach.x > rightReach && 
				// 	rightReach > rule.RightHandReach.y) {
					return false;
				}
				else if (!inLeftZone && !rule.LeftHandReach.Contains(rightReach)) {
				// else if (!inLeftZone &&
				// 			rule.LeftHandReach.x > rightReach &&
				// 			rightReach > rule.LeftHandReach.y) {
					return false;
				}
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
	public bool EitherHandAngle;
	public AngleRange LeftHandAngles;
	public AngleRange RightHandAngles;
	// public Vector2 LeftHandAngles;
	// public Vector2 RightHandAngles;
	public bool RequireHandReach;
	public bool EitherHandReach;
	public Range LeftHandReach;
	public Range RightHandReach;
	// public Vector2 LeftHandReach;
	// public Vector2 RightHandReach;
	public bool Waving;

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