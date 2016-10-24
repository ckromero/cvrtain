using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HeadState {Upright, Bow, Curtsy, None};
public class HeadTracker : MonoBehaviour {

    public HeadState HeadState {
        get {
            if (_StateBuffer.Count > 0) {
                return _StateBuffer[0];
            }
            return HeadState.None;
        }
    }

    public List<HeadState> HeadStateList
    {
        get
        {
        	if (_StateBuffer.Count > 0) {
        		return _StateBuffer;
        	}
        	var returnList = new List<HeadState>(1);
        	returnList.Add(HeadState.None);
            return returnList;
        }
    }

	public OneWayCollider BowCollider;
	public OneWayCollider CurtsyCollider;
	public ColliderSubscriber UprightCollider;

	private List<HeadState> _StateBuffer;

	void Awake() {
		BowCollider.OnTriggerEnter += OnBowTriggerEnter;
		BowCollider.OnTriggerExit += OnBowTriggerExit;
		CurtsyCollider.OnTriggerEnter += OnCurtsyTriggerEnter;
		CurtsyCollider.OnTriggerExit += OnCurtsyTriggerExit;
		UprightCollider.OnTriggerEnterDelegate += OnUprightTriggerEnter;
		UprightCollider.OnTriggerExitDelegate += OnUprightTriggerExit;

		_StateBuffer = new List<HeadState>();

		// _StateBuffer.Add(HeadState.Upright);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnBowTriggerEnter(Collider other) {
		_StateBuffer.Add(HeadState.Bow);
	}

	void OnBowTriggerExit(Collider other) {
		_StateBuffer.Remove(HeadState.Bow);
	}

	void OnCurtsyTriggerEnter(Collider other) {
		_StateBuffer.Add(HeadState.Curtsy);
	}

	void OnCurtsyTriggerExit(Collider other) {
		_StateBuffer.Remove(HeadState.Curtsy);
	}

	void OnUprightTriggerEnter(Collider other) {
		_StateBuffer.Add(HeadState.Upright);
	}

	void OnUprightTriggerExit(Collider other) {
		_StateBuffer.Remove(HeadState.Upright);
	}
}
