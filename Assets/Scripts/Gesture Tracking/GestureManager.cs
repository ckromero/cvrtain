using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FRL.IO;

public class GestureManager : MonoBehaviour, IGestureManager
{

	public Text TestOutputText;
	public Gesture[] Gestures;
	public float GestureLockoutDuration = 0.5f;
	private float _RemainingLockout;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;

	private bool _WaitingForReset = false;
	private List<Gesture> _Gestures;

	private float _ClearTextTimer;

	public CompletedGestureStruct LastGesture { get; private set; }

	void Awake () {
		LastGesture = new CompletedGestureStruct ("", 0f);
	}

	// Use this for initialization
	void Start () {
		_HeadTracker = GetComponent<HeadTracker> ();
		_HandsTracker = GetComponent<HandsTracker> ();
		_Gestures = new List<Gesture> (Gestures);
	}
	
	// Update is called once per frame
	void Update () {
		/* update all gestures and sort them by how completed they are. This
		* means that more complex, partially completed rules are evaluated
		* first to ensure that simpler rules do not override them. */

		if (!_HandsTracker.Working || !_HeadTracker.Working) {
			return;
		}

		_ClearTextTimer -= Time.deltaTime;
		if (_ClearTextTimer <= 0f) {
			TestOutputText.text = "";
		}

		if (_RemainingLockout > 0f) _RemainingLockout -= Time.deltaTime;

		var largestCompletion = 0;
		var name = "";
		foreach (var gesture in Gestures) {
			// Debug.Log("updating gesture " + gesture.Name);
			gesture.GestureUpdate (_HeadTracker, _HandsTracker);
			if (gesture.Completed) {
				if (_RemainingLockout <= 0f) {
					if (largestCompletion < gesture.RuleIndex) {
						largestCompletion = gesture.RuleIndex;
						name = gesture.Name;
					}
				}
				gesture.Reset();
			}
		}
		if (largestCompletion > 0) {
			var message = "COMPLETED: " + name;
			Debug.Log(message);
			TestOutputText.text = message;
			LastGesture = new CompletedGestureStruct (name, Time.time);
			_ClearTextTimer = 1f;
			_RemainingLockout = GestureLockoutDuration;
		}
	}

	public bool CompareGestureNames (string[] names) {
		var correct = true;
		foreach (var name in names) {
			var matchingGesture = false;
			foreach (var gesture in Gestures) {
				if (name.ToLower().Trim() == gesture.Name.ToLower().Trim()) {
					matchingGesture = true;
					break;
				}
			}
			if (!matchingGesture) {
				correct = false;
				Debug.Log ("ERROR: " + name + " is not a valid Gesture name!!!!");
			}
		}
		return correct;
	}

}

/* contains the name of the gesture and the time that it was completed */
public struct CompletedGestureStruct {
	public readonly string Name;
	public readonly float Time;

	public CompletedGestureStruct (string name, float time)
	{
		this.Name = name;
		this.Time = time;
	}
}