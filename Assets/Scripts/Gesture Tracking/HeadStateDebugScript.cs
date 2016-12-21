using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadStateDebugScript : MonoBehaviour {

	public HeadTracker HeadTracker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var bufferState = "headstates: ";
        foreach(var state in HeadTracker.HeadStateList)
        {
            bufferState += state + ", ";
        }
        Debug.Log(bufferState);
        GetComponent<Text>().text = bufferState;
	}
}
