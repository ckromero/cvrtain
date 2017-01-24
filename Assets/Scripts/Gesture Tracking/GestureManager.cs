﻿﻿using UnityEngine;
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

	public float WeirdDanceRequirement;
	public float MovementTrackingWindow;

	private Vector3[] _TrackedRandomPositions;
	private int _HandIndex;

	private HeadTracker _HeadTracker;
	private HandsTracker _HandsTracker;
	private GestureCallibrator _Callibrator;

	private float _ClearTextTimer;

	public CompletedGestureStruct LastGesture { get; private set; }

	void Awake () {
	}

	void Start () {
		_HeadTracker = GetComponent<HeadTracker> ();
		_HandsTracker = GetComponent<HandsTracker> ();
		_Callibrator = GetComponent<GestureCallibrator>();
	}
	
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
				/* if the lockout time is over and another gesture has not already
				* been detected this frame, register this gesture */
				if (_RemainingLockout <= 0f) {
					if (name == string.Empty) {
						name = gesture.Name;
					}
				}
				/* gestures should be reset regardless of whether or not they will
				* be registerd to prevent state from backing up or overlapping */
				gesture.Reset();
			}
            /* if this gesture is being delayed and another gesture has been completed
            * this frame, reset the delayed gesture */
            else if (gesture.DelayedComplete && name != string.Empty)
            {
                gesture.Reset();
            }
		}
		if (name != string.Empty) {
			DetectedGesture(name);
		}
	}

	void FixedUpdate() {
		if (!_HandsTracker.Working || !_HeadTracker.Working || !Tracking) {
			return;
		}

		var leftPos = _HandsTracker.LeftHand.position;
		var rightPos = _HandsTracker.RightHand.position;
		/* hand positions are translated into local space to account for the gesture
		* tracker being scaled by the callibration process */
		_TrackedRandomPositions[_HandIndex] = transform.InverseTransformPoint(leftPos);
		_TrackedRandomPositions[_HandIndex+1] = transform.InverseTransformPoint(rightPos);

		_HandIndex += 2;
		var length = _TrackedRandomPositions.Length;
		if (_HandIndex >= _TrackedRandomPositions.Length) {
			_HandIndex = 0;
		}

		/* iterate over _TrackedRandomPositions and add up the deltas between
		* all saved positions. If the total distance is greater than 
		* WeirdDanceRequirement, fire the Weird dance gesture */
		var randomDistance = 0f;
		for (var i = _HandIndex - 1; i != _HandIndex; i--) {
            if (i < 0) {
                i = length - 1;
                if (i == _HandIndex) {
                    break;
                }
            }
            /* index2 should be 2 away from index since each hand's positions
            * are saved in alternating indices */
            var index = i;
            var index2 = (index <= 1) ? length - 1 - index : index - 2;
            var pos1 = _TrackedRandomPositions[index];
            var pos2 = _TrackedRandomPositions[index2];
            randomDistance += Vector3.Distance(pos1, pos2);
		}

		if (randomDistance >= WeirdDanceRequirement && _RemainingLockout <= 0f) {
			DetectedGesture("Weird dance");
		}
	}

	/* Handles all code around processing a completed gesture.
	* Has the gesture been perfomred 4 or more times in a row and a 
	* Laughable should be fired instead? Was the gesure performed while
	* the player was facing away from the audience?
	* Also handles basic housekeeping like setting the value of LastGesture */
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
        else if (_HeadTracker.Facing == HeadFacing.Back) {
        	/* the very first time the player performs away from the audience
        	* fire the First Backwards gesture. Otherwise, Laughable should be
        	* triggered instead */
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

		/* random tracking should be cleared so that it doesn't fire
		* due to movement caused by another, completed gesture */
		for (var i = 0; i < _TrackedRandomPositions.Length; i++) {
			var position = _HandsTracker.LeftHand.position;
			if (i % 2 == 1) {
				position = _HandsTracker.RightHand.position;
			}
			_TrackedRandomPositions[i] = _HandsTracker.transform.InverseTransformDirection(position);
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
		_TrackedRandomPositions = new Vector3[(int)handMoveCount];
		_HandIndex = 0;

		PerformedGestures = new List<CompletedGestureStruct>();
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