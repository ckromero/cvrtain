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
		if (_WaitingForReset) {
			if (_HeadTracker.HeadState == HeadState.Upright) {
				_WaitingForReset = false;
                Debug.Log("completed: Reset");
                TestOutputText.text = "";
                foreach (var gesture in Gestures) {
                	gesture.Reset();
                }
            }
			else {
				return;
			}
		}

		// Debug.Log(LastGesture.Name);

		/* update all gestures and sort them by how completed they are. This
		* means that more complex, partially completed rules are evaluated
		* first to ensure that simpler rules do not override them. */
		var sortedGestures = new List<Gesture>();
        foreach (var gesture in Gestures) {
            gesture.GestureUpdate(_HeadTracker, _HandsTracker);
			if (gesture.Completed) {
				LastGesture = new CompletedGestureStruct(gesture.Name, Time.time);

				var message = "COMPLETED: " + gesture.Name;
				// Debug.Log(message);
				TestOutputText.text = message;
				_WaitingForReset = true;
				break;
			}
			for (var i = 0; i <= sortedGestures.Count; i++) {
				if (i == sortedGestures.Count) {
					sortedGestures.Add(gesture);
					break;
				}
				else if (gesture.CurrentRuleIndex > sortedGestures[i].CurrentRuleIndex) {
					sortedGestures.Insert(i, gesture);
					break;
				}
			}
		}
		/* since the above loop can break prematurely, make sure that all
		* gestures have actually been added to sortedGestures */
		if (sortedGestures.Count == Gestures.Length) {
			Gestures = sortedGestures.ToArray();
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

public struct CompletedGestureStruct {
	public readonly string Name;
	public readonly float Time;

	public CompletedGestureStruct(string name, float time) {
		this.Name = name;
		this.Time = time;
	}
}