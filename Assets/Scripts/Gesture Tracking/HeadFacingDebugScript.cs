using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadFacingDebugScript : MonoBehaviour {

	public HeadTracker HeadTracker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Text>().text = "Head Facing: " + HeadTracker.Facing;		
	}
}
