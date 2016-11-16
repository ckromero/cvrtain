using UnityEngine;
using System.Collections;
using FRL.IO;

public class TriggerListener : MonoBehaviour, IGlobalTriggerHandler {

	private int _TriggersDown = 0;
	[SerializeField]
	private float _TimeBudget;

	private float _InputWindowTime = -1f;

	public float LastDoublePress { get; private set; }

	// Use this for initialization
	void Start () {
		LastDoublePress = Mathf.Infinity;	
	}
	
	// Update is called once per frame
	void Update () {
		if (_InputWindowTime < 0f) {
			return;
		}
		_InputWindowTime -= Time.deltaTime;
	}

	public void OnGlobalTriggerPressDown(ViveControllerModule.EventData e) {
		TriggerDown();
	}

	public void OnGlobalTriggerPress(ViveControllerModule.EventData e) {
	}

	public void OnGlobalTriggerPressUp(ViveControllerModule.EventData e) {
		TriggerUp();
	}

	public void OnGlobalTriggerTouchDown(ViveControllerModule.EventData e) {
	}

	public void OnGlobalTriggerTouch(ViveControllerModule.EventData e) {
	}

	public void OnGlobalTriggerTouchUp(ViveControllerModule.EventData e) {
	}

	void TriggerDown() {
		_TriggersDown++;
		if (_TriggersDown == 1) {
			_InputWindowTime = _TimeBudget;
		}
		else if (_TriggersDown == 2 && _InputWindowTime > 0f) {
			LastDoublePress = Time.timeSinceLevelLoad;
			_InputWindowTime = -1f;
		}
	}

	void TriggerUp() {
		_TriggersDown--;
		_InputWindowTime = -1f;
	}

}
