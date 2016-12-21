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

	public HeadFacing Facing {
		get {
			return _HeadTracker.Facing;
		}
	}

	public bool Tracking { get; set; }
	public bool WeirdRandomMovement { get; private set; }

	public float HoldStateRequirement;
	public float WeirdDanceRequirement;
	public float MovementTrackingWindow;

	private Vector3[] _TrackedHandPositions;
	private int _HandIndex;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;

	private float _ClearTextTimer;

	public CompletedGestureStruct LastGesture { get; private set; }

	void Awake () {
		Reset();
	}

	// Use this for initialization
	void Start () {
		_HeadTracker = GetComponent<HeadTracker> ();
		_HandsTracker = GetComponent<HandsTracker> ();
	}
	
	// Update is called once per frame
	void Update () {
		/* update all gestures and sort them by how completed they are. This
		* means that more complex, partially completed rules are evaluated
		* first to ensure that simpler rules do not override them. */

		if (!_HandsTracker.Working || !_HeadTracker.Working || !Tracking) {
			return;
		}

		_ClearTextTimer -= Time.deltaTime;
		if (_ClearTextTimer <= 0f) {
			TestOutputText.text = "";
		}

		if (_RemainingLockout > 0f) _RemainingLockout -= Time.deltaTime;

		var largestCompletion = 0;
		var name = string.Empty;
		foreach (var gesture in Gestures) {
			gesture.GestureUpdate (_HeadTracker, _HandsTracker);
			if (gesture.Completed) {
				if (_RemainingLockout <= 0f) {
					if (name == string.Empty) {
					// if (largestCompletion < gesture.RuleIndex) {
					// 	largestCompletion = gesture.RuleIndex;
						name = gesture.Name;
					// }
					}
				}
				gesture.Reset();
			}
		}
		if (name != string.Empty) {
		// if (largestCompletion > 0) {
			DetectedGesture(name);
		}
	}

	void FixedUpdate() {
		if (!_HandsTracker.Working || !_HeadTracker.Working || !Tracking) {
			return;
		}

		var leftPos = _HandsTracker.LeftHand.position;
		var rightPos = _HandsTracker.RightHand.position;

		_TrackedHandPositions[_HandIndex] = leftPos;
		_TrackedHandPositions[_HandIndex+1] = rightPos;

		_HandIndex += 2;
		if (_HandIndex >= _TrackedHandPositions.Length) {
			_HandIndex = 0;
		}

		var totalDistance = 0f;
		var length = _TrackedHandPositions.Length;
		for (var i = _HandIndex - 1; i != _HandIndex; i--) {
            if (i < 0) {
                i = length - 1;
                if (i == _HandIndex) {
                    break;
                }
            }
            var index = i;
            var index2 = (index <= 1) ? length - 1 - index : index - 2;
            var pos1 = _TrackedHandPositions[index];
            var pos2 = _TrackedHandPositions[index2];
            totalDistance += Vector3.Distance(pos1, pos2);
		}

		WeirdRandomMovement = (totalDistance >= HoldStateRequirement);

		if (totalDistance >= WeirdDanceRequirement && _RemainingLockout <= 0f) {
			DetectedGesture("Weird dance");
		}
	}

	public void DetectedGesture(string name) {
		var message = "COMPLETED: " + name;
		Debug.Log(message);
		TestOutputText.text = message;
		LastGesture = new CompletedGestureStruct (name, Time.time);
		_ClearTextTimer = 1f;
		_RemainingLockout = GestureLockoutDuration;
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

	public void Reset() {
		foreach (var gesture in Gestures) {
			gesture.Reset();
		}
		LastGesture = new CompletedGestureStruct ("", 0f);
		var handMoveCount = MovementTrackingWindow * 60 * 2;
		_TrackedHandPositions = new Vector3[(int)handMoveCount];
		_HandIndex = 0;
		Tracking = false;
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