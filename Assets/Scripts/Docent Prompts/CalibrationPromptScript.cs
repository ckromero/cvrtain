using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationPromptScript : MonoBehaviour {

	public GestureCallibrator Calibrator;
	private Text _Text;

	// Use this for initialization
	void Start () {
		_Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Calibrator.Callibrated) {
			_Text.text = "To Play: Face the CVRTAIN. Hands at your sides. Press a thumb button";
		}		
		else {
			_Text.text = "";
		}
	}
}
