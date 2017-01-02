﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FRL.IO;

public class GestureManager : MonoBehaviour, IGestureManager
{

	public Text TestOutputText;
	public Text DocentText;
	public Gesture[] Gestures;
	public float GestureLockoutDuration = 0.5f;
	private float _RemainingLockout;
	public int MaximumStreakLength;

	public List<CompletedGestureStruct> PerformedGestures { get; private set; }
	public Dictionary<string, int> GestureCounts {
		get {
			var dictionary = new Dictionary<string, int>();
			foreach (var gesture in PerformedGestures) {
				if (dictionary.ContainsKey(gesture.Name)) {
					dictionary[gesture.Name]++;
				}
				else {
					dictionary.Add(gesture.Name, 1);
				}
			}
			return dictionary;
		}
	}

	public HeadFacing Facing {
		get {
			return _HeadTracker.Facing;
		}
	}

    private bool _Tracking = false;
	public bool Tracking {
		get {
			return _Tracking;
		}
		set {
			if (_Tracking != value) {
				if (!value) {
					_Callibrator.Reset();
				}
				else {
					Debug.Log("I should be disabling the callibrator");
                    if(_Callibrator == null)
                    {
                        Debug.Log("I don't have a callibrator attached");
                    }
					_Callibrator.Disabled = true;
                   Reset();
				}
			}
			_Tracking = value;
		}
	}
	public bool WeirdRandomMovement { get; private set; }

	public float HoldStateRequirement;
	public float WeirdDanceRequirement;
	public float MovementTrackingWindow;

	// private Vector3[] _TrackedHandPositions;
	private Vector3[] _TrackedRandomPositions;
	private Vector3[] _TrackedDancePositions;
	private int _HandIndex;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;
	private GestureCallibrator _Callibrator;

	private float _ClearTextTimer;

	public CompletedGestureStruct LastGesture { get; private set; }

	void Awake () {
	}

	// Use this for initialization
	void Start () {
		_HeadTracker = GetComponent<HeadTracker> ();
		_HandsTracker = GetComponent<HandsTracker> ();
		_Callibrator = GetComponent<GestureCallibrator>();
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
			DocentText.text = "";
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
            /* if a gesture is waiting on its delay, but another gesture has been completed
             * go ahead and reset the waiting gesture */
            else if (gesture.DelayedComplete && name != string.Empty)
            {
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

		// _TrackedHandPositions[_HandIndex] = transform.InverseTransformPoint(leftPos);
		// _TrackedHandPositions[_HandIndex+1] = transform.InverseTransformPoint(rightPos);

		_TrackedRandomPositions[_HandIndex] = transform.InverseTransformPoint(leftPos);
		_TrackedRandomPositions[_HandIndex+1] = transform.InverseTransformPoint(rightPos);

		_HandIndex += 2;
		var length = _TrackedRandomPositions.Length;
		// if (_HandIndex >= _TrackedHandPositions.Length) {
		if (_HandIndex >= _TrackedRandomPositions.Length) {
			_HandIndex = 0;
		}

		var randomDistance = 0f;
		var danceDistance = 0f;
		// var length = _TrackedHandPositions.Length;
		for (var i = _HandIndex - 1; i != _HandIndex; i--) {
            if (i < 0) {
                i = length - 1;
                if (i == _HandIndex) {
                    break;
                }
            }
            var index = i;
            var index2 = (index <= 1) ? length - 1 - index : index - 2;
            // var pos1 = _TrackedHandPositions[index];
            // var pos2 = _TrackedHandPositions[index2];
            var pos1 = _TrackedRandomPositions[index];
            var pos2 = _TrackedRandomPositions[index2];
            randomDistance += Vector3.Distance(pos1, pos2);

            pos1 = _TrackedDancePositions[index];
            pos2 = _TrackedDancePositions[index2];
            danceDistance += Vector3.Distance(pos1, pos2);
		}

		// WeirdRandomMovement = (randomDistance >= HoldStateRequirement);

		if (randomDistance >= WeirdDanceRequirement && _RemainingLockout <= 0f) {
			DetectedGesture("Weird dance");
			//for (var i = 0; i < _TrackedDancePositions.Length; i++) {
			//	_TrackedDancePositions[i] = Vector3.zero;
			//}
		}
		else if (randomDistance >= HoldStateRequirement && _RemainingLockout <= 0f) {
			DetectedGesture("Weird random");
			//for (var i = 0; i < _TrackedRandomPositions.Length; i++) {
			//	_TrackedRandomPositions[i] = Vector3.zero;
			//}
		}
	}

	public void DetectedGesture(string name) {

		var streak = 0;
		for (var i = PerformedGestures.Count - 1; i >= 0; i--) {
			var testName = PerformedGestures[i].Name;
			if (testName == name) {
				streak++;
				if (streak >= MaximumStreakLength) {
					break;
				}
			}
			else if (testName != name && testName != "Laughable") {
				break;
			}
		}

		if (streak >= MaximumStreakLength) {
			name = "Laughable";
		}
        else if (_HeadTracker.Facing == HeadFacing.Back)
        {
            name = "First Backwards";
            foreach (var completed in PerformedGestures)
            {
                if (completed.Name == "First Backwards")
                {
                    name = "Laughable";
                    break;
                }
            }
        }

		var message = "COMPLETED: " + name;
		Debug.Log(message);
		TestOutputText.text = message;
		DocentText.text = message;
		LastGesture = new CompletedGestureStruct (name, Time.time);
		_ClearTextTimer = 1f;
		_RemainingLockout = GestureLockoutDuration;

		PerformedGestures.Add(LastGesture);

        /* clear the random trackers */

		for (var i = 0; i < _TrackedDancePositions.Length; i++) {
			_TrackedDancePositions[i] = Vector3.zero;
		}
		for (var i = 0; i < _TrackedRandomPositions.Length; i++) {
			_TrackedRandomPositions[i] = Vector3.zero;
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

	public void Reset() {
		foreach (var gesture in Gestures) {
			gesture.Reset();
		}
		LastGesture = new CompletedGestureStruct("", 0f);
		var handMoveCount = MovementTrackingWindow * 60 * 2;
		// _TrackedHandPositions = new Vector3[(int)handMoveCount];
		_TrackedRandomPositions = new Vector3[(int)handMoveCount];
		_TrackedDancePositions = new Vector3[(int)handMoveCount];
		_HandIndex = 0;

		PerformedGestures = new List<CompletedGestureStruct>();

        /* this is a hack to fix the bug requiring a double bow to start performance */
        //_HeadTracker.ForceUpright();
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