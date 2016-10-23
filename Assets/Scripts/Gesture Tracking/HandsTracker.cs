using UnityEngine;
using System.Collections;

public class HandsTracker : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;

	public int Divisions = 8;
	public int Rings = 2;
	public float RingRadius = 0.65f;

	// public int LeftHandZone { get; private set; }

	public int LeftHandZone {
		get {
			var localPosition = LeftHand.localPosition;
			return CalculateZone(new Vector2(localPosition.y, localPosition.z));
		}
	}

	public int RightHandZone {
		get {
			var localPosition = RightHand.localPosition;
			return CalculateZone(new Vector2(localPosition.y, localPosition.z));
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
			distance = Mathf.Clamp(distance/RingRadius, 0, Rings);
			return (int)Mathf.Floor(distance) + 1;
		}
	}

	public int RightHandRing {
		get {
			var distance = Vector3.Distance(RightHand.position, transform.position);
			distance = Mathf.Clamp(distance/RingRadius, 0, Rings);
			return (int)Mathf.Floor(distance) + 1;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
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
		for (var i = 1; i <= Rings; i++) {
			var color = Color.Lerp(Color.yellow, Color.magenta, (i-1f)/(Rings - 1f));
			Gizmos.color = color;
			Gizmos.DrawWireSphere(Vector3.zero, i * RingRadius);
		}
		Gizmos.matrix = oldMatrix;
	}
}
