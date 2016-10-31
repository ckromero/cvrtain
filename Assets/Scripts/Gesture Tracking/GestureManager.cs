using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GestureManager : MonoBehaviour, IGestureManager {

	public Text TestOutputText;
	public Gesture[] Gestures;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;

	private bool _WaitingForReset = false;
	private List<Gesture> _Gestures;

	public CompletedGestureStruct LastGesture { get; private set; }

	void Awake() {
		LastGesture = new CompletedGestureStruct("", 0f);
	}

	// Use this for initialization
	void Start () {
		_HeadTracker = GetComponent<HeadTracker>();
		_HandsTracker = GetComponent<HandsTracker>();
		_Gestures = new List<Gesture>(Gestures);
	}
	
	// Update is called once per frame
	void Update () {
		/* update all gestures and sort them by how completed they are. This
		* means that more complex, partially completed rules are evaluated
		* first to ensure that simpler rules do not override them. */
		var largestCompletion = 0;
        foreach (var gesture in Gestures) {
            gesture.GestureUpdate(_HeadTracker, _HandsTracker);
			if (gesture.Completed) {
				if (largestCompletion < gesture.CurrentRuleIndex) {
					LastGesture = new CompletedGestureStruct(gesture.Name, Time.time);
					largestCompletion = gesture.CurrentRuleIndex;
				}
				var message = "COMPLETED: " + gesture.Name;
				Debug.Log(message);
				gesture.Reset();
			}
		}
	}

	public bool CompareGestureNames(string[] names) {
		var correct = true;
		foreach (var name in names) {
			var matchingGesture = false;
			foreach (var gesture in Gestures) {
				if (name == gesture.Name) {
					matchingGesture = true;
					break;
				}
			}
			if (!matchingGesture) {
				correct = false;
			}
			Debug.Log("ERROR: " + name + " is not a valid Gesture name!!!!");
		}
		return correct;
	}
}

/* contains the name of the gesture and the time that it was completed */
public struct CompletedGestureStruct {
	public readonly string Name;
	public readonly float Time;

	public CompletedGestureStruct(string name, float time) {
		this.Name = name;
		this.Time = time;
	}
}