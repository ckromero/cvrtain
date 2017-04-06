using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocentPrompter : MonoBehaviour {

	public Text CalibrationText;
	public Text ExperienceOverText;
	public GestureCallibrator Calibrator;

	private GesturesCompletedScript _CompletedScript;
	private GameObject promptImage;

	void Start() {
		_CompletedScript = GetComponentInChildren<GesturesCompletedScript>();
		promptImage = this.gameObject.transform.Find ("Calibration prompt").gameObject.transform.Find ("PromptImage").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Calibrator.Callibrated) {
			promptImage.SetActive (true);
//			CalibrationText.text = "To Play: Face the CVRTAIN. Hands at your sides. Press a thumb button.";
		}		
		else {
			promptImage.SetActive (false);
//			CalibrationText.text = "";
		}
	}

	public void ExperienceOver() {
		ExperienceOverText.text = "Performance over";
		_CompletedScript.DisplayGestures();
	}

	public void ExperienceReset() {
		ExperienceOverText.text = "";
		_CompletedScript.HideGestures();
	}

}
