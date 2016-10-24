using UnityEngine;
using System.Collections;

public class GestureCallibrator : MonoBehaviour {

	public KeyCode CallibrationKey;
	public float HeadOffset;
	public Transform HeadTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(CallibrationKey)) {
			Callibrate();
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
