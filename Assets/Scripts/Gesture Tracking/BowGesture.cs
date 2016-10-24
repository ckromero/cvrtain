using UnityEngine;
using System.Collections;

public class BowGesture : MonoBehaviour {

	private GestureDetector _GestureDetector;

	private bool _Active;

	void Awake() {
		_Active = false;
	}

	// Use this for initialization
	void Start () {
		_GestureDetector = GetComponent<GestureDetector>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_Active) {
			if (_GestureDetector.HeadState == HeadState.Upright) {
				_Active = false;
			}
		}
		else {
			if (_GestureDetector.HeadState == HeadState.Bow) {
				_Active = true;
				if (_GestureDetector.LeftHandZone == 1 && _GestureDetector.RightHandZone == -1) {
					Debug.Log("<color=green>The audience goes wild!!!!</color>");
				}
				else {
					Debug.Log("<color=blue>polite applause</color>");
				}
			}
		}
	}
}
