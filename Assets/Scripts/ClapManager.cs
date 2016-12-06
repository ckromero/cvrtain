using UnityEngine;
using System.Collections;

public class ClapManager: MonoBehaviour {
	public ClapControl[] clapControls;

	public void UpdateClappers(string update) { 
		foreach (ClapControl clapper in clapControls) {
			if (update=="triggerGoToHigh")
				clapper.triggerGoToHigh = true;
			if (update=="triggerRaisedFist")
				clapper.triggerRaisedFist= true;
			if (update=="triggerHighClapping")
				clapper.triggerHighClapping = true;
		}
	}

	public void UpdateClappers(ClapTrigger trigger) {
		foreach (var clapper in clapControls) {
			if (trigger == ClapTrigger.GoToHigh)
				clapper.triggerGoToHigh = true;
			if (trigger == ClapTrigger.RaisedFist)
				clapper.triggerRaisedFist= true;
			if (trigger == ClapTrigger.HighClapping)
				clapper.triggerHighClapping = true;
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public enum ClapTrigger {
	PoliteClapping,
	GoToHigh,
	RaisedFist,
	HighClapping
}