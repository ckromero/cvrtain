using UnityEngine;
using System.Collections;

public class HandsTracker : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;

	public int Divisions = 8;
	public int Rings = 2;
	public float RingRadius = 0.65f;
	public float MinimumRadius = 0.65f;

	// public int LeftHandZone { get; private set; }

	public int LeftHandZone {
		get {
			var localPosition = transform.InverseTransformPoint(LeftHand.position);
			return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
		}
	}

	public int RightHandZone {
		get {
			var localPosition = transform.InverseTransformPoint(RightHand.position);
			return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
		}
	}

	private int CalculateZone(Vector2 position) {
		var angle = Mathf.Atan2(position.y, position.x);
		var zone = (int)Mathf.Floor(angle / (Mathf.PI * 2 / Divisions));
		if (zone >= 0) zone++;
		return zone;
	}

	public int LeftHandRing {
		get {
			var distance = Vector3.Distance(LeftHand.position, transform.position);
			distance = Mathf.Clamp((distance-MinimumRadius)/RingRadius, 0, Rings);
			return (int)Mathf.Floor(distance) + 1;
		}
	}

	public int RightHandRing {
		get {
			var distance = Vector3.Distance(RightHand.position, transform.position);
			distance = Mathf.Clamp((distance-MinimumRadius)/RingRadius, 0, Rings);
			return (int)Mathf.Floor(distance) + 1;
		}
	}

	void Start () {
	
	}
	
	void Update () {

	}
	
	void OnDrawGizmos() {
		for (var i = 0; i < Divisions; i++) {
			var angle = i * Mathf.PI / (Divisions / 2);

			var target = Vector3.zero;
			target.y = 5 * Mathf.Cos(angle);
			target.z = 5 * Mathf.Sin(angle);

			var start = transform.position;
			target = transform.TransformPoint(target);

			UnityEngine.Gizmos.DrawLine(start, target);
		}

		var oldMatrix = Gizmos.matrix;
		var cubeMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = oldMatrix * cubeMatrix;
		for (var i = 0; i < Rings; i++) {
			var color = Color.Lerp(Color.yellow, Color.magenta, i/(Rings - 1f));
			Gizmos.color = color;
			Gizmos.DrawWireSphere(Vector3.zero, i * RingRadius + MinimumRadius);
		}
		Gizmos.matrix = oldMatrix;
	}
}
