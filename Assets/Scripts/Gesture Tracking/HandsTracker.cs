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
            var localPos = transform.InverseTransformPoint(LeftHand.position);
            var distance = Vector3.Distance(localPos, Vector3.zero);
			distance = Mathf.Ceil((distance-MinimumRadius)/RingRadius);
			distance = Mathf.Clamp(distance, 0, Rings);
			return (int)distance + 1;
		}
	}

	public int RightHandRing {
		get {
            var localPos = transform.InverseTransformPoint(RightHand.position);
            var distance = Vector3.Distance(localPos, Vector3.zero);
			distance = Mathf.Ceil((distance-MinimumRadius)/RingRadius);
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

	public bool Working {
		get {
			return (LeftHand != null && RightHand != null);
		}
	}

    public float LastWave { get; private set; }

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
		if (Waving) {
			Debug.Log("I'm waving");
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
		RaycastHit hit;
		/* raycast around the center of the hand. If none of the casts hit the
		* ceiling or the floor, then the hand is being held upright and we
		* can assume that's good enough for waving */			
		var directions = new Vector3[4] {
											handTransform.up,
											handTransform.up * -1,
											handTransform.right,
											handTransform.right * -1
										};
		var position = handTransform.position;
		foreach (var direction in directions) {
			if (Physics.Raycast(position, direction, out hit, 100f, WavingLayers)) {
				var hitObject = hit.collider.gameObject;
				var facing = hitObject.GetComponent<HeadLookReceiver>().Facing;
				if (facing == HeadFacing.Floor || facing == HeadFacing.Ceiling) {
					return false;
				}
			}
		}
		/* raycast from the palm of the hand to ensure that the hand is actually
		* facing the audience when the player waves */
		// var down = handTransform.up * -1;
		// if (Physics.Raycast(position, down, out hit, 100f, WavingLayers)) {
		// 	var hitObject = hit.collider.gameObject;
		// 	if (hitObject.GetComponent<HeadLookReceiver>()) {
		// 		var facing = hitObject.GetComponent<HeadLookReceiver>().Facing;
		// 		if (facing != HeadFacing.Front &&
		// 				facing != HeadFacing.Left &&
		// 				facing != HeadFacing.Right) {
		// 			return false;
		// 		}
		// 	}
		// 	else {
		// 		return false;
		// 	}
		// }

		var maxWorldDelta = 0f;
		var maxLocalDelta = 0f;
		var totalWorldDistance = 0f;
		var totalLocalDistance = 0f;

		/* convert the positions to 2D local space. Local positions are tracked
		* in 2D to ensure that the hand movements are not triggering off of Z
		* axis movement. */
		var localPositions = new Vector2[positions.Length];
		for (var i = 0; i < positions.Length; i++) {
			var localPos = transform.InverseTransformPoint(positions[i]);
			localPositions[i] = new Vector2(localPos.x, localPos.y);
		}


		/* the wave check takes the first point and looks for another point
		* sufficiently far away. It then checks to see if there is a third
		* point after that that is also far enough away and also closer to
		* the first point */

		var firstIndex = _CurrentIndex;
		var secondIndex = -1;
		var thirdIndex = -1;

		/* get the averaged scaling of the player so you can adjust the
		* value of the movement */
		var localAdjust = 0f;
		for (var i = 0; i < 3; i++) {
			localAdjust += transform.lossyScale[i];
		}
		localAdjust /= 3;

		for (var i = _CurrentIndex - 1; i != _CurrentIndex; i--) {
            if (i < 0) {
                i = positions.Length - 1;
                if (i == _CurrentIndex) {
                    break;
                }
            }
            var index = i;
            var index2 = (index == 0) ? positions.Length - 1 : index - 1;
			//var index = i - _CurrentIndex;
			//if (index >= positions.Length) index -= positions.Length;
   //         var index2 = (index + 1 >= positions.Length) ? 0 : index + 1;
			totalWorldDistance += Vector3.Distance(
													positions[index],
													positions[index2]
												);
			totalLocalDistance += Vector2.Distance(
													localPositions[index],
													localPositions[index2]
												);

			if (secondIndex < 0) {
                var delta = Vector3.Distance(
                                                positions[firstIndex],
                                                positions[index]
                                            );
                //var delta = Mathf.Abs(positions[firstIndex].x - positions[index].y);
				//var localDelta = Vector3.Distance(
				//									localPositions[firstIndex],
				//									localPositions[index]
				//								);
                var localDelta = Mathf.Abs(localPositions[firstIndex].x - localPositions[index].x);
                localDelta *= localAdjust;
                var worldBounded = WaveLimits.x <= delta && delta <= WaveLimits.y;
                var localBounded = WaveLimits.x <= localDelta && localDelta <= WaveLimits.y;
                if (worldBounded && localBounded) {
				//if (delta > maxWorldDelta && localDelta > maxLocalDelta) {
					// Debug.Log("found a second index");
					secondIndex = index;
				}
			}
			else {
                var delta = Vector3.Distance(
                                                positions[secondIndex],
                                                positions[index]
                                            );
                //var localDelta = Vector3.Distance(
                //									localPositions[secondIndex],
                //									localPositions[index]
                //								);
                //var delta = Mathf.Abs(positions[secondIndex].x - positions[index].y);
                var localDelta = Mathf.Abs(localPositions[secondIndex].x - localPositions[index].x);
				localDelta *= localAdjust;
                var worldBounded = WaveLimits.x <= delta && delta <= WaveLimits.y;
                var localBounded = WaveLimits.x <= localDelta && localDelta <= WaveLimits.y;
                if (worldBounded && localBounded)
                {
                    //if (delta > maxWorldDelta && localDelta > maxLocalDelta) {
					// Debug.Log("attempting third point check");
					var firstDelta = Vector3.Distance(
														positions[firstIndex],
														positions[index]
													);

                    //Debug.Log("first delta: " + firstDelta.ToString("F4"));
                    var firstLocalDelta = Mathf.Abs(localPositions[firstIndex].x - localPositions[index].x);
                    firstLocalDelta *= localAdjust;
                    // Debug.Log("first delta: " + firstDelta + " second delta: " + delta);
                    if ((firstDelta < delta && Mathf.Abs(firstDelta - delta) >= delta / 2) && 
                        (firstLocalDelta < localDelta && Mathf.Abs(firstLocalDelta - localDelta) >= localDelta / 2)) {
                        thirdIndex = index;
                        break;
					}
				}
			}
		}

		/* the wave needs to check for movement in both world and local space to
		* ensure that movement is not being caused simply by rotating the gesture
		* tracking zone.
        * Local space checking is needed to ensure the controller is
        * being moved along approximately the same plane as the head rather than
        * forward and backward.
        * The total distance check is to make sure that the hand has moved enough
        * in the window that it can be considered a wave. */
		// var returnValue = (maxWorldDelta >= WaveLimits.x &&
		// 					maxWorldDelta <= WaveLimits.y &&
		// 					maxLocalDelta >= WaveLimits.x &&
		// 					maxLocalDelta <= WaveLimits.y &&
		// 					totalLocalDistance > WaveDistance &&
		// 					totalWorldDistance > WaveDistance);
		// var returnValue = (thirdIndex >= 0 &&
		// 					totalLocalDistance > WaveDistance &&
		// 					totalWorldDistance > WaveDistance);
		var returnValue = (thirdIndex >= 0);
        if (returnValue) {
        	Debug.Log("this should be a wave");
            LastWave = Time.deltaTime;
            // for (var i = 0; i < positions.Length; i++) {
            //     positions[i] = handTransform.position;
            // }
        }
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
		var cubeMatrix = Matrix4x4.TRS(
											transform.position,
											transform.rotation,
											transform.lossyScale
										);
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
