using UnityEngine;
using System.Collections;

public class GestureCallibrator : MonoBehaviour {

	public KeyCode CallibrationKey;
	public float HeadOffset;
	public Transform HeadTransform;

	public SteamVR_TrackedObject LeftWand;
	public SteamVR_TrackedObject RightWand;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        try
        {
            var leftDevice = SteamVR_Controller.Input((int)LeftWand.index);
            var rightDevice = SteamVR_Controller.Input((int)RightWand.index);
            if (leftDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) ||
                rightDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Callibrate();
            }
        }
        catch (System.IndexOutOfRangeException e)
        {
            if (Input.GetKeyDown(CallibrationKey))
            {
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
        Debug.Log("callibrate!");
		var headPosition = HeadTransform.localPosition;
		/* this allows callibration if the project is being run with a Vive */
		if (HeadTransform.parent.gameObject.name == "Camera (head)") {
			headPosition = HeadTransform.parent.localPosition;
		}
		var myPosition = transform.localPosition;
		var yDiff = headPosition.y / (myPosition.y + HeadOffset);
		var scale = Vector3.one * yDiff;
		transform.localScale = scale;
		myPosition.y *= yDiff;
		transform.localPosition = myPosition;
	}
}
