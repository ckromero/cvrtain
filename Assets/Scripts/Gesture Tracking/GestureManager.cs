using UnityEngine;
using System.Collections;

public class GestureManager : MonoBehaviour {

	public Gesture[] Gestures;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;

	private bool _WaitingForReset = false;

	// Use this for initialization
	void Start () {
		_HeadTracker = GetComponent<HeadTracker>();
		_HandsTracker = GetComponent<HandsTracker>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_WaitingForReset) {
			if (_HeadTracker.HeadState == HeadState.Upright) {
				_WaitingForReset = false;
			}
			else {
				return;
			}
		}

		foreach (var gesture in Gestures) {
			if (gesture.GestureUpdate(_HeadTracker, _HandsTracker)) {
				Debug.Log("completed: " + gesture.Name);
				_WaitingForReset = true;
				// break;
			}
		}	
	}
}
