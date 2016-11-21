using UnityEngine;
using System.Collections;

public class DebugEnabler : MonoBehaviour {

	public GameObject Head;
	public GameObject LeftHand;
	public GameObject RightHand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetupDebug() {
		var headTarget = transform.FindChild("Debug Head Target");
		var leftTarget = transform.FindChild("Debug Left Hand Target");
		var rightTarget = transform.FindChild("Debug Right Hand Target");

		Head.SetActive(true);
		Head.transform.position = headTarget.position;
		LeftHand.SetActive(true);
		LeftHand.transform.position = leftTarget.position;
		RightHand.SetActive(true);
		RightHand.transform.position = rightTarget.position;
	}
}
