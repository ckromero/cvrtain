using UnityEngine;
using System.Collections;

public class HandsTracker : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;

	public int Divisions = 8;
	public int Rings = 2;
	public float RingRadius = 0.65f;
	public float MinimumRadius = 0.65f;

	public Vector2 WaveLimits;
	public float WaveDistance = 1f;

	private Vector2[] _TrackedLeftPositions = new Vector2[30];
	private Vector2[] _TrackedRightPositions = new Vector2[30];

	private int _PositionIndex = 0;

	// public int LeftHandZone { get; private set; }

	public int LeftHandZone {
		get {
			var localPosition = transform.InverseTransformPoint(LeftHand.position);
			var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
			return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
		}
	}

	public int RightHandZone {
		get {
			var localPosition = transform.InverseTransformPoint(RightHand.position);
			var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
			return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
		}
	}

	public float LeftHandAngle {
		get {
			var localPosition = transform.InverseTransformPoint(LeftHand.position);
			var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
			return -1 * CalculateAngle(adjustedPosition);
		}
	}

	public float RightHandAngle {
		get {
			var localPosition = transform.InverseTransformPoint(RightHand.position);
			var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
			return CalculateAngle(adjustedPosition);
		}
	}

	private float CalculateAngle(Vector2 position) {
		var angle = Mathf.Atan2(position.y, position.x);
		var degrees = angle * Mathf.Rad2Deg;
		return degrees;
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
			distance = Mathf.Ceil((distance-MinimumRadius)/RingRadius);
			// distance = Mathf.Clamp((distance-MinimumRadius)/RingRadius, 0, Rings + 1);
			distance = Mathf.Clamp(distance, 0, Rings);
			return (int)distance + 1;
		}
	}

	public int RightHandRing {
		get {
			var distance = Vector3.Distance(RightHand.position, transform.position);
			distance = Mathf.Ceil((distance-MinimumRadius)/RingRadius);
			// distance = Mathf.Clamp((distance-MinimumRadius)/RingRadius, 0, Rings + 1);
			distance = Mathf.Clamp(distance, 0, Rings);
			return (int)distance + 1;
		}
	}

	public bool LeftHandWaving { get; private set; }
	public bool RightHandWaving { get; private set; }
	public bool Waving {
		get {
			return LeftHandWaving || RightHandWaving;
		}
	}

	void Start () {
	
	}
	
	void Update () {
		if (Waving) {
			Debug.Log("I'm waving!!!");
		}
	}

	void LateUpdate() {
		/* check for wave tracking */
		var leftLocal = transform.InverseTransformPoint(LeftHand.position);
		var rightLocal = transform.InverseTransformPoint(RightHand.position);
		var leftHand = new Vector2(leftLocal.z, leftLocal.y);
		var rightHand = new Vector2(rightLocal.z, rightLocal.y);

		_TrackedLeftPositions[_PositionIndex] = leftHand;
		_TrackedRightPositions[_PositionIndex] = rightHand;
		_PositionIndex++;

		if (_PositionIndex >= _TrackedLeftPositions.Length) {
			_PositionIndex = 0;
		}

		LeftHandWaving = CheckForWave(_TrackedLeftPositions);
		RightHandWaving = CheckForWave(_TrackedRightPositions);

	}

	private bool CheckForWave(Vector2[] positions) {
		var maxDelta = 0f;
		var totalDistance = 0f;

		for (var i = 0; i < positions.Length - 1; i++) {
			totalDistance += Vector3.Distance(positions[i], positions[i+1]);
			for (var j = i + 1; j < positions.Length; j++) {
				var distance = Vector3.Distance(positions[i], positions[j]);
				if (distance > maxDelta) {
					maxDelta = distance;
				}
			}
		}

		var returnValue = (maxDelta >= WaveLimits.x &&
				maxDelta <= WaveLimits.y &&
				totalDistance > WaveDistance);

		// if (returnValue) {
		// 	Debug.Log("waving with delta of: " + maxDelta +
		// 			" and distance of: " + totalDistance);
		// }

		return returnValue;
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
