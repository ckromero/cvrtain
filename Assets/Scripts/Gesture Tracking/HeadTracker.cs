using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum HeadState {Upright, Bow, Curtsy, None};
public enum HeadFacing {Left, Front, Right, Floor, Back, Ceiling, None};
public class HeadTracker : MonoBehaviour {

    public HeadState HeadState {
        get {
            if (_StateBuffer.Count > 0) {
                return _StateBuffer[0];
            }
            return HeadState.None;
        }
    }

    public bool Working {
    	get {
    		return (HeadTransform != null);
    	}
    }

    public HeadFacing Facing { get; private set; }

    public bool FacingFloor { get; private set; }
    public bool FacingCeiling { get; private set; }

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
	public Transform HeadTransform;

	private List<HeadState> _StateBuffer;
	[SerializeField]
	private LayerMask _LookMask;

	void Awake() {
		_StateBuffer = new List<HeadState>();
	}

	// Use this for initialization
	void Start () {
		BowCollider.OnTriggerEnter += OnBowTriggerEnter;
		BowCollider.OnTriggerExit += OnBowTriggerExit;
		CurtsyCollider.OnTriggerEnter += OnCurtsyTriggerEnter;
		CurtsyCollider.OnTriggerExit += OnCurtsyTriggerExit;
		UprightCollider.OnTriggerEnterDelegate += OnUprightTriggerEnter;
		UprightCollider.OnTriggerExitDelegate += OnUprightTriggerExit;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		var position = HeadTransform.position;
		// var direction = HeadTransform.forward;
		// direction.x = 0f;

		var x = HeadTransform.rotation.eulerAngles.x;
		if (x > 180) x = 0;
		var t = Mathf.Clamp01(x / 90);

		var forward = HeadTransform.forward;
		var up = HeadTransform.up;
		var direction = Vector3.Lerp(forward, up, t);

		if (Physics.Raycast(position, direction, out hit, _LookMask)) {
            try {
                var newFacing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
                if (newFacing == HeadFacing.Floor) {
                	newFacing = Facing;
                }
                else {
                	Facing = newFacing;
                }
                // if (Facing == HeadFacing.Floor) {
                //     direction = HeadTransform.up;
                //     if (Physics.Raycast(position, direction, out hit, _LookMask)) {
                //         Facing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
                //     }
                // }
            }
            catch (NullReferenceException e) {
            	Facing = HeadFacing.None;
            }
		}
		if (Physics.Raycast(position, forward, out hit, _LookMask)) {
            try
            {
                var facing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
                if (facing == HeadFacing.Floor)
                {
                    FacingFloor = true;
                }
                else
                {
                    FacingFloor = false;
                }
                if (facing == HeadFacing.Ceiling)
                {
                    FacingCeiling = true;
                }
                else
                {
                    FacingCeiling = false;
                }
            } catch (NullReferenceException e)
            {
                //Debug.Log("I SHOULD NOT BE CAST AGAINST: " + hit.collider.gameObject.name);
            }
		}
        //Debug.Log("Current head state: " + HeadState);
        var bufferState = "headstates: ";
        foreach(var state in _StateBuffer)
        {
            bufferState += state + ", ";
        }
        Debug.Log(bufferState);
	}

	void OnBowTriggerEnter(Collider other) {
		InsertState(HeadState.Bow);
	}

	void OnBowTriggerExit(Collider other) {
		RemoveState(HeadState.Bow);
	}

	void OnCurtsyTriggerEnter(Collider other) {
		InsertState(HeadState.Curtsy);
	}

	void OnCurtsyTriggerExit(Collider other) {
		RemoveState(HeadState.Curtsy);
	}

	void OnUprightTriggerEnter(Collider other) {
		InsertState(HeadState.Upright);
	}

	void OnUprightTriggerExit(Collider other) {
		RemoveState(HeadState.Upright);
	}

	void InsertState(HeadState state) {
        var index = 0;
        if (!_StateBuffer.Contains(state)) {
			_StateBuffer.Add(state);
			index = _StateBuffer.Count - 1;
        }
        else {
        	index = _StateBuffer.IndexOf(state);
        }
        _StateBuffer.RemoveAt(index);
        _StateBuffer.Insert(0, state);
	}

	void RemoveState(HeadState state) {
		while (_StateBuffer.Remove(state)) {}
	}

    public void Clear() {
        _StateBuffer.Clear();
    }

    void OnDrawGizmos() {
		var position = HeadTransform.position;

		// Debug.Log("rotation: " + HeadTransform.rotation.eulerAngles.x);
		var x = HeadTransform.rotation.eulerAngles.x;
		if (x > 180) x = 0;
		var t = Mathf.Clamp01(x / 90);

		var forward = HeadTransform.forward;
		var up = HeadTransform.up;
		var direction = Vector3.Lerp(forward, up, t);

		// Debug.Log("clamp rotation t: " + t);

		Gizmos.color = Color.red;
		Gizmos.DrawRay(position, direction);
    }
}
