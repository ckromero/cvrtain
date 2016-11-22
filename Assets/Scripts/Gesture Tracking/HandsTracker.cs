using UnityEngine;
using System.Collections;

public class HandsTracker : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;

	public LayerMask WavingLayers;

	public int Divisions = 8;
	public int Rings = 2;
	public float RingRadius = 0.65f;
	public float MinimumRadius = 0.65f;

	public Vector2 WaveLimits;
	public float WaveDistance = 1f;

	private Vector2[] _TrackedLeftPositions = new Vector2[30];
	private Vector2[] _TrackedRightPositions = new Vector2[30];

	public float TrackingWindow;

	private Vector3[] _LeftHandPositions;
	private Vector3[] _RightHandPositions;
	private float[] _LeftHandAngles;
	private float[] _RightHandAngles;

	int _CurrentIndex = -1;

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
			var adjustedPosition = new Vector2(localPosition.y, localPosition.x);
			return -1 * CalculateAngle(adjustedPosition);
		}
	}

	public float RightHandAngle {
		get {
			var localPosition = transform.InverseTransformPoint(RightHand.position);
			var adjustedPosition = new Vector2(localPosition.y, localPosition.x);
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

	void Awake() {
		var size = (int)Mathf.Floor(TrackingWindow * 60);
		_LeftHandPositions = new Vector3[size];
		_RightHandPositions = new Vector3[size];
		_LeftHandAngles = new float[size];
		_RightHandAngles = new float[size];
	}

	void Start () {
		/* populate the angle arrays */
		for (var i = 0; i < _LeftHandAngles.Length; i++) {
			_LeftHandAngles[i] = LeftHandAngle;
			_RightHandAngles[i] = RightHandAngle;	
		}
		// foreach (var angle in _LeftHandAngles) {
		// 	angle = LeftHandAngle;
		// }	
		// foreach (var angle in _RightHandAngles) {
		// 	angle = RightHandAngle;
		// }	
	}
	
	void Update () {
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

		// LeftHandWaving = CheckForWave(_TrackedLeftPositions);
		// RightHandWaving = CheckForWave(_TrackedRightPositions);
	}

	void FixedUpdate() {
		_CurrentIndex++;
		if (_CurrentIndex >= _LeftHandPositions.Length) {
			_CurrentIndex = 0;
		}
		_LeftHandPositions[_CurrentIndex] = LeftHand.position;
		_RightHandPositions[_CurrentIndex] = RightHand.position;
		_LeftHandAngles[_CurrentIndex] = LeftHandAngle;
		_RightHandAngles[_CurrentIndex] = RightHandAngle;

		LeftHandWaving = CheckForWave(LeftHand, _LeftHandPositions);
		RightHandWaving = CheckForWave(RightHand, _RightHandPositions);
	}

	private bool CheckForWave(Transform handTransform, Vector3[] positions) {
		/* raycast from the palm of the hand to ensure that the hand is actually
		* facing the audience when the player waves */
		RaycastHit hit;
		var down = handTransform.up * -1;
		if (Physics.Raycast(handTransform.position, down, out hit, 100f, WavingLayers)) {
			var hitObject = hit.collider.gameObject;
			if (hitObject.GetComponent<HeadLookReceiver>()) {
				var facing = hitObject.GetComponent<HeadLookReceiver>().Facing;
				if (facing != HeadFacing.Front &&
						facing != HeadFacing.Left &&
						facing != HeadFacing.Right) {
					return false;
				}
			}
			else {
				return false;
			}
		}

		var maxWorldDelta = 0f;
		var maxLocalDelta = 0f;
		var totalWorldDistance = 0f;
		var totalLocalDistance = 0f;

		var localPositions = new Vector2[positions.Length];
		for (var i = 0; i < positions.Length; i++) {
			var localPos = transform.InverseTransformPoint(positions[i]);
			// Debug.Log(localPos.x);
			localPositions[i] = new Vector2(localPos.x, localPos.y);
		}

		for (var i = 0; i < positions.Length - 1; i++) {
			totalWorldDistance += Vector3.Distance(positions[i], positions[i+1]);
			totalLocalDistance += Vector2.Distance(localPositions[i], localPositions[i+1]);
			for (var j = i + 1; j < positions.Length; j++) {
				var delta = Vector3.Distance(positions[i], positions[j]);
				if (delta > maxWorldDelta) {
					maxWorldDelta = delta;
				}

				delta = Vector2.Distance(localPositions[i], localPositions[j]);
				if (delta > maxLocalDelta) {
					maxLocalDelta = delta;
				}
			}
		}

		// Debug.Log("maxWorldDelta: " + maxWorldDelta + " totalWorldDistance: " + totalWorldDistance);
		// Debug.Log("maxLocalDelta: " + maxLocalDelta + " totalLocalDistance: " + totalLocalDistance);

		/* the wave needs to check for movement in both world and local space to
		* ensure that movement is not being caused simply by rotating the gesture
		* tracking zone */
		var returnValue = (maxWorldDelta >= WaveLimits.x &&
							maxWorldDelta <= WaveLimits.y &&
							maxLocalDelta >= WaveLimits.x &&
							maxLocalDelta <= WaveLimits.y &&
							totalLocalDistance > WaveDistance &&
							totalWorldDistance > WaveDistance);
		return returnValue;
	}

	private bool CheckForWave(float[] angles) {
		if (_CurrentIndex < 0) {
			return false;
		}
		var haltPoint = _CurrentIndex + 1;
		if (haltPoint >= angles.Length) haltPoint = 0;
		var index = _CurrentIndex - 2;
		if (index < 0) index = angles.Length - index;
		var reversalCount = 0;
		// Debug.Log("current index: " + _CurrentIndex);
		// while (index > 0) {
		// var testOutput = "Tracked positions: \n";
		while (index != haltPoint) {
			var i = index;
			var j = (i + 1 >= angles.Length) ? 0 : i + 1;
			var k = (j + 1 >= angles.Length) ? 0 : j + 1;	
			var lastDelta = angles[i] - angles[j];
			var thisDelta = angles[j] - angles[k];
			// if (k == _CurrentIndex) {
			// 	Debug.Log("this index offset correctly");
			// }

			// Debug.Log("last delta: " + lastDelta + " this delta: " + thisDelta);

			var lastDirection = (int)(lastDelta/Mathf.Abs(lastDelta));
			if (lastDelta.Equals(0f)) {
				// Debug.Log("fire lastDelta reset");
				lastDirection = 1;
			}
			var thisDirection = (int)(thisDelta/Mathf.Abs(thisDelta));
			if (thisDelta.Equals(0f)) {
				// Debug.Log("fire thisDelta reset");
				thisDirection = 1;
			}
			if (lastDirection != thisDirection) {
				Debug.Log("last: " + lastDirection + " this: " + thisDirection);
				reversalCount++;
			}

			// testOutput += testOutput + ", ";

			index = (index - 1 < 0) ? angles.Length - 1: index - 1;	
			// Debug.Log("current index is: " + index);
		}

		// Debug.Log(testOutput);

		return reversalCount >= 5;
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
			target.x = 5 * Mathf.Sin(angle);

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

		/* draw rays for the palms of the hands */

		Gizmos.color = Color.red;
		Gizmos.DrawRay(LeftHand.position, LeftHand.up * -1);
		Gizmos.DrawRay(RightHand.position, RightHand.up * -1);
	}
}
