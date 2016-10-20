using UnityEngine;
using System.Collections;

public class HeadScript : MonoBehaviour {

	public HeadState State { get; private set; }

	void Awake() {
		State = HeadState.Upright;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		State = HeadState.Bow;
	}

	void OnTriggerExit() {
		State = HeadState.Upright;
	}
}
