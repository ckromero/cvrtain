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
        var text = "Current Level: " + LevelManager.CurrentLevel;
        if (LevelManager.Failed)
        {
            text = "FAILED";
        }
        else if (LevelManager.Failing) {
            text = "Failing";
        }
		GetComponent<Text>().text = text;
	}
}
