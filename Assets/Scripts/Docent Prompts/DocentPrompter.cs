using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocentPrompter : MonoBehaviour {

	public Text CalibrationText;
	public Text ExperienceOverText;
	public GestureCallibrator Calibrator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!Calibrator.Callibrated) {
			CalibrationText.text = "PLEASE CALIBRATE";
		}		
		else {
			CalibrationText.text = "";
		}
	}

	public void ExperienceOver() {
		ExperienceOverText.text = "Performance over";
	}

	public void ExperienceReset() {
		ExperienceOverText.text = "";
	}

}
