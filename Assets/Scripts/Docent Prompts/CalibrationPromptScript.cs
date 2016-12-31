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
			_Text.text = "PLEASE CALIBRATE";
		}		
		else {
			_Text.text = "";
		}
	}
}
