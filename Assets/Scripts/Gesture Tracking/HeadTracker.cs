using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum HeadState {Upright, Bow, Curtsy};
public class HeadTracker : MonoBehaviour {

	public HeadState HeadState {
		get {
			if (_StateBuffer.Count > 0) {
				return _StateBuffer[0];
			}
			return HeadState.Upright;
		}
	}

	public OneWayCollider BowCollider;
	public OneWayCollider CurtsyCollider;

	private List<HeadState> _StateBuffer;

	void Awake() {
		BowCollider.OnTriggerEnter += OnBowTriggerEnter;
		BowCollider.OnTriggerExit += OnBowTriggerExit;
		CurtsyCollider.OnTriggerEnter += OnCurtsyTriggerEnter;
		CurtsyCollider.OnTriggerExit += OnCurtsyTriggerExit;

		_StateBuffer = new List<HeadState>();
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
}
