using UnityEngine;

[System.Serializable]
public class Gesture {

	public string Name;
	public GestureRule[] Rules;

	public int CurrentRuleIndex { get; private set; }
	public bool Completed {
		get {
			return CurrentRuleIndex >= Rules.Length - 1 && _OnARule;
		}
	}

	private float _TimeToNextRule = Mathf.Infinity;
	private float _TimeLeftOnRule = Mathf.Infinity;
	private bool _OnARule = false;

	public bool GestureUpdate(HeadTracker head, HandsTracker hands) {

		if (CurrentRuleIndex >= Rules.Length) {
			Debug.Log("ERROR: " + Name + " has overrun rules");
			return false;
		}
		var currentRule = Rules[CurrentRuleIndex];

		var ruleComplete = true;
		//if (currentRule.HeadState != head.HeadState) {
		//	ruleComplete = false;
		//}
        if (currentRule.HeadState != HeadState.None && !head.HeadStateList.Contains(currentRule.HeadState))
        {
            ruleComplete = false;
        }

		if (currentRule.LeftHandZone != 0 || !ruleComplete) {
			if (currentRule.LeftHandZone != hands.LeftHandZone) {
				ruleComplete = false;
			}
		}

		if (currentRule.RightHandZone != 0 || !ruleComplete) {
			if (currentRule.RightHandZone != hands.RightHandZone) {
				ruleComplete = false;
			}
		}

		if (currentRule.LeftHandRing != 0 || !ruleComplete) {
			if (currentRule.LeftHandRing != hands.LeftHandRing) {
				ruleComplete = false;
			}
		}

		if (currentRule.RightHandRing != 0 || !ruleComplete) {
			if (currentRule.RightHandRing != hands.RightHandRing) {
				ruleComplete = false;
			}
		}

		if (ruleComplete) {
			if (!_OnARule) {
				_OnARule = true;
				var time = currentRule.TimeLimit;
				_TimeLeftOnRule = (time.Equals(0f)) ? Mathf.Infinity : time;
				Debug.Log("completed rule " + CurrentRuleIndex + " of " + Name);
			}
			_TimeLeftOnRule -= Time.deltaTime;
			if (_TimeLeftOnRule <= 0f) {
				Debug.Log(Name + " -- timed out on rule " + CurrentRuleIndex);
				_TimeLeftOnRule = Mathf.Infinity;
				CurrentRuleIndex = 0;
				_OnARule = false;
			}
			// CurrentRuleIndex++;
			// if (CurrentRuleIndex == Rules.Length) {
			// 	CurrentRuleIndex = 0;
			// 	return true;
			// }
			// Debug.Log("advancing rule of " + Name);
			// var delay = currentRule.RuleGap;
			// _TimeToNextRule = (delay.Equals(0f)) ? Mathf.Infinity : delay;
		}
		else {
			if (_OnARule) {
				_OnARule = false;
				CurrentRuleIndex++;
				var delay = currentRule.RuleGap;
				_TimeToNextRule = (delay.Equals(0f)) ? Mathf.Infinity : delay;
			}
			_TimeToNextRule -= Time.deltaTime;
			if (_TimeToNextRule <= 0f) {
				Debug.Log(Name + " -- timed out getting to rule " + CurrentRuleIndex);
				CurrentRuleIndex = 0;
				_TimeToNextRule = Mathf.Infinity;
			}
		}

		return false;
	}

	public void Reset() {
		CurrentRuleIndex = 0;
		_OnARule = false;
		_TimeLeftOnRule = Mathf.Infinity;
		_TimeToNextRule = Mathf.Infinity;
	}
}

[System.Serializable]
public class GestureRule {
	public HeadState HeadState;
	public int LeftHandZone;
	public int LeftHandRing;
	public int RightHandZone;
	public int RightHandRing;
	public float TimeLimit;
	public float RuleGap;
}