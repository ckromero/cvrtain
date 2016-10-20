using UnityEngine;

[System.Serializable]
public class Gesture {

	public string Name;
	public GestureRule[] Rules;

	public int CurrentRuleIndex { get; private set; }
	public bool Completed { get; private set; }

	private float TimeToNextRule = Mathf.Infinity;

	public bool GestureUpdate(HeadTracker head, HandsTracker hands) {
		TimeToNextRule -= Time.deltaTime;
		if (TimeToNextRule <= 0f) {
			CurrentRuleIndex = 0;
		}

		var currentRule = Rules[CurrentRuleIndex];

		var ruleComplete = true;
		if (currentRule.HeadState != head.HeadState) {
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
			CurrentRuleIndex++;
			if (CurrentRuleIndex == Rules.Length) {
				CurrentRuleIndex = 0;
				return true;
			}
			Debug.Log("advancing rule of " + Name);
			var delay = currentRule.Delay;
			TimeToNextRule = (delay.Equals(0f)) ? Mathf.Infinity : delay;
		}

		return false;
	}
}

[System.Serializable]
public class GestureRule {
	public HeadState HeadState;
	public int LeftHandZone;
	public int LeftHandRing;
	public int RightHandZone;
	public int RightHandRing;
	public float Delay;
}