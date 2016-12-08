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
		var direction = HeadTransform.forward;
		if (Physics.Raycast(position, direction, out hit, _LookMask)) {
            try {
                Facing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
                if (Facing == HeadFacing.Floor) {
                    direction = HeadTransform.up;
                    if (Physics.Raycast(position, direction, out hit, _LookMask)) {
                        Facing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
                    }
                }
            }
            catch (NullReferenceException e) {
            	Facing = HeadFacing.None;
            }
		}
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
		Debug.Log("collide?");
		_StateBuffer.Add(HeadState.Upright);
	}

	void OnUprightTriggerExit(Collider other) {
		_StateBuffer.Remove(HeadState.Upright);
	}
}
