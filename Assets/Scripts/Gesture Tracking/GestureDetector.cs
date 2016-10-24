using UnityEngine;
using System.Collections;

public class GestureDetector : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;
	public HeadScript Head;

	const int _DIVISIONS = 8;

	public HeadState HeadState {
		get {
			return Head.State;
		}
	}
	public int LeftHandZone { get; private set; }
	public int RightHandZone { get; private set; }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var leftHand = Mathf.Atan2(LeftHand.localPosition.z, LeftHand.localPosition.y);
		var rightHand = Mathf.Atan2(RightHand.localPosition.z, RightHand.localPosition.y);

		LeftHandZone = (int)Mathf.Floor(leftHand / (Mathf.PI / 4));
		RightHandZone = (int)Mathf.Floor(rightHand / (Mathf.PI / 4));

		if (LeftHandZone >= 0) LeftHandZone++;
		if (RightHandZone >= 0) RightHandZone++;

		// Debug.Log("left hand is in zone " + LeftHandZone + " and right is in " + RightHandZone);
	}
	
	void OnDrawGizmos() {
		for (var i = 0; i < _DIVISIONS; i++) {
			var angle = i * Mathf.PI / (_DIVISIONS / 2);

			var target = Vector3.zero;
			target.y = 5 * Mathf.Cos(angle);
			target.z = 5 * Mathf.Sin(angle);

			var start = transform.position;
			target = transform.TransformPoint(target);

			UnityEngine.Gizmos.DrawLine(start, target);
		}

	}
}

