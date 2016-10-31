using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Gesture {

	public string Name;
	public GestureRule[] Rules;

	public int CurrentRuleIndex { get; private set; }
	public bool Completed {
		get {
			return CurrentRuleIndex >= Rules.Length - 1;
		}
	}

	private float _TimeToNextRule = Mathf.Infinity;
	private float _TimeLeftOnRule = Mathf.Infinity;

	public Gesture() {
		CurrentRuleIndex = -1;
		Rules = new GestureRule[2];
	}

	public void GestureUpdate(HeadTracker head, HandsTracker hands) {

		if (Completed) {
			return;
		}

		if (CheckRule(head, hands, Rules[CurrentRuleIndex+1])) {
			CurrentRuleIndex++;
			Debug.Log("Completed rule: " + CurrentRuleIndex + " of " + Name);
			if (!Completed) {
				var rule = Rules[CurrentRuleIndex];
				var time = (rule.HasMaximumDuration) ? rule.MaxDuration : Mathf.Infinity;
				_TimeLeftOnRule = time;
				time = (rule.HasTimeLimit) ? rule.TimeLimit : Mathf.Infinity;
				_TimeToNextRule = time;
			}
		}
		else if (CurrentRuleIndex >= 0 && CheckRule(head, hands, Rules[CurrentRuleIndex])) {
			_TimeLeftOnRule -= Time.deltaTime;
			if (_TimeLeftOnRule <= 0f) {
				Debug.Log(Name + " -- timed out on rule " + CurrentRuleIndex);
				Reset();
			}
		}
		else {
			_TimeToNextRule -= Time.deltaTime;
			if (_TimeToNextRule <= 0f) {
				Debug.Log(Name + " -- timed out getting to rule " + CurrentRuleIndex);
				Reset();
			}
		}
	}

	public void Reset() {
		CurrentRuleIndex = -1;
		_TimeLeftOnRule = Mathf.Infinity;
		_TimeToNextRule = Mathf.Infinity;
	}

	private bool CheckRule(HeadTracker head, HandsTracker hands, GestureRule rule) {
		var ruleComplete = true;
		if (rule.RequireHeadState) {
			if (!head.HeadStateList.Contains(rule.HeadState)) {
				ruleComplete = false;
			}
		}	

        if (rule.RequireHandAngles) {
        	var leftZone = hands.LeftHandZone;
        	var rightZone = hands.RightHandZone;
        	if (rule.LeftHandAngles.x < leftZone ||
        			leftZone < rule.LeftHandAngles.y) {
        		ruleComplete = false;
        	}
        	if (rule.RightHandAngles.x > rightZone ||
        			rightZone > rule.RightHandAngles.y) {
        		ruleComplete = false;
        	}
        }

		if (rule.RequireHandReach) {
			var leftReach = hands.LeftHandRing;
			var rightReach = hands.RightHandRing;
			if (rule.LeftHandReach.x > leftReach ||
				rule.LeftHandReach.y < leftReach) {
				ruleComplete = false;
			}
			if (rule.RightHandReach.x > rightReach ||
				rule.RightHandReach.y < rightReach) {
				ruleComplete = false;
			}
		}

		return ruleComplete;
	}
}

[System.Serializable]
public class GestureRule {
	public bool RequireHeadState;
	public HeadState HeadState;
	public bool RequireHandAngles;
	public Vector2 LeftHandAngles;
	public Vector2 RightHandAngles;
	public bool RequireHandReach;
	public Vector2 LeftHandReach;
	public Vector2 RightHandReach;

	public bool HasMaximumDuration;
	public float MaxDuration;
	public bool HasTimeLimit;
	public float TimeLimit;

	public GestureRule() {}
}	