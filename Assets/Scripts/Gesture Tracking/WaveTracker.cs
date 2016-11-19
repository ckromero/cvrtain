using UnityEngine;
using System.Collections;

public class WaveTracker : MonoBehaviour {

	public float TrackingWindow;

	Vector3[] _LeftHandPositions;
	Vector3[] _RightHandPositions;
	float[] _LeftHandAngles;
	float[] _RightHandAngles;

	int _CurrentIndex = 0;

	void Awake() {
		var size = (int)Mathf.Floor(TrackingWindow * 60);
		_LeftHandPositions = new Vector3[size];
		_RightHandPositions = new Vector3[size];
		_LeftHandAngles = new float[size];
		_RightHandAngles = new float[size];
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate() {

	}
}
