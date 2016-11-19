using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HeadState {Upright, Bow, Curtsy, None};
public enum HeadFacing {Left, Front, Right, Floor, Back};
public class HeadTracker : MonoBehaviour {

    public HeadState HeadState {
        get {
            if (_StateBuffer.Count > 0) {
                return _StateBuffer[0];
            }
            return HeadState.None;
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

		// _StateBuffer.Add(HeadState.Upright);
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
    var states = "HeadStates: ";
    foreach (var HeadState in _StateBuffer) {
      states += HeadState + ", ";
    }
//    Debug.Log(states);
		RaycastHit hit;
		if (Physics.Raycast(HeadTransform.position, HeadTransform.forward, out hit, _LookMask)) {
			// Debug.Log("I am looking at: " + hit.collider.gameObject.name);
		//	Facing = hit.collider.GetComponent<HeadLookReceiver>().Facing;
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
		_StateBuffer.Add(HeadState.Upright);
	}

	void OnUprightTriggerExit(Collider other) {
		_StateBuffer.Remove(HeadState.Upright);
	}
}
