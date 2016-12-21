using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeirdMovementDebugScript : MonoBehaviour {

	public GestureManager gestureManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gestureManager.WeirdRandomMovement) {
			GetComponent<Text>().text = "weird random";
		}
		else {
			GetComponent<Text>().text = "";
		}
	}
}
