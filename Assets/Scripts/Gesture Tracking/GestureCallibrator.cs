using UnityEngine;
using System.Collections;

public class GestureCallibrator : MonoBehaviour {

	public KeyCode CallibrationKey;
	public float HeadOffset;
	public Transform HeadTransform;

	public SteamVR_TrackedObject LeftWand;
	public SteamVR_TrackedObject RightWand;

	private SteamVR_Controller.Device _LeftDevice;
	private SteamVR_Controller.Device _RightDevice;
	private bool _WandsEnabled;

	// Use this for initialization
	void Start () {
		try {
			_LeftDevice = SteamVR_Controller.Input((int)LeftWand.index);	
			_RightDevice = SteamVR_Controller.Input((int)RightWand.index);
			_WandsEnabled = true;
		}
		catch (System.IndexOutOfRangeException e) {
			_WandsEnabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_WandsEnabled) {
			if (_LeftDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) ||
				_RightDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
				Callibrate();
			}
		}
		else {
			if (Input.GetKeyDown(CallibrationKey)){
				Callibrate();
			}	
		}
	}

	void OnDrawGizmos() {
		var position = transform.position;
		position.y += HeadOffset * transform.lossyScale.y;
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(position, 0.1f);
	}

	public void Callibrate() {
		var headPosition = HeadTransform.localPosition;
		var myPosition = transform.localPosition;
		var yDiff = headPosition.y / (myPosition.y + HeadOffset);
		var scale = Vector3.one * yDiff;
		transform.localScale = scale;
		myPosition.y *= yDiff;
		transform.localPosition = myPosition;
	}
}
