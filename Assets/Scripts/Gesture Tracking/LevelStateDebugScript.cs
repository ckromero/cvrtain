using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStateDebugScript : MonoBehaviour {

	public LevelsManager LevelManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var text = "";
		if (LevelManager.Performing) {
	        text = "Current Level: " + LevelManager.CurrentLevel;
		}
		GetComponent<Text>().text = text;
	}
}
