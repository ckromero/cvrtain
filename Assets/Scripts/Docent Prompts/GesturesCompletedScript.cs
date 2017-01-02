using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GesturesCompletedScript : MonoBehaviour {

	public GestureManager gestureManager;

	private Text _Text;

	// Use this for initialization
	void Start () {
		_Text = GetComponent<Text>();
		_Text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DisplayGestures() {
		var message = "Gestures Completed:\n";
		var keys = new string[gestureManager.GestureCounts.Count];
		gestureManager.GestureCounts.Keys.CopyTo(keys, 0);
		for (var i = keys.Length - 1; i >= 0; i--) {
			for (var j = 0; j < i; j++) {
				if (String.Compare(keys[j], keys[j+1]) > 0) {
					var temp = (string)keys[j+1];
					keys[j+1] = keys[j];
					keys[j] = temp;
				}
			}
		}
		foreach (var key in gestureManager.GestureCounts.Keys) {
			var count = gestureManager.GestureCounts[key];
			message += count + "x " + key + "\n";
		}
		if (gestureManager.GestureCounts.Count == 0) {
			message = "";
		}
		_Text.text = message;
	}

	public void HideGestures() {
		_Text.text = "";
	}
}
