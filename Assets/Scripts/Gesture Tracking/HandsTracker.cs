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

	public float TrackingWindow;

	private Vector3[] _LeftHandPositions;
	private Vector3[] _RightHandPositions;
	private float[] _LeftHandAngles;
	private float[] _RightHandAngles;

	int _CurrentIndex = -1;

	/* Zones are the distances of the hands from the center of the GestureTracker */

	// public int LeftHandZone {
	// 	get {
	// 		var localPosition = transform.InverseTransformPoint(LeftHand.position);
	// 		var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
	// 		return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
	// 	}
	// }

	// public int RightHandZone {
	// 	get {
	// 		var localPosition = transform.InverseTransformPoint(RightHand.position);
	// 		var adjustedPosition = new Vector2(localPosition.y, localPosition.z);
	// 		return -1 * CalculateZone(new Vector2(localPosition.y, localPosition.z));
	// 	}
	// }

	/* Angles are the angle of the hands relative to the center of the GestureTracker */

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

	/* Rings are the distances of the hands from the center of the GestureTracker */

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
	}
	
	void Update () {
		if (Waving) {
			Debug.Log("I'm waving");
		}
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

	/* iterate over the saved history of a hand's positions looking for a 
	* back and forth motion that will be counted as a wave */
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

		var maxWorldDelta = 0f;
		var maxLocalDelta = 0f;
		var totalWorldDistance = 0f;
		var totalLocalDistance = 0f;

		/* convert the positions to 2D local space. Local positions are tracked
		* in 2D to ensure that waves only trigger on side to side and up & down
		* movements, excluding foward & backward */
		var localPositions = new Vector2[positions.Length];
		for (var i = 0; i < positions.Length; i++) {
			var localPos = transform.InverseTransformPoint(positions[i]);
			localPositions[i] = new Vector2(localPos.x, localPos.y);
		}

		/* to find a wave, take the most recent hand position (at _CurrentIndex)
		* and look back for another point that is a distance away within the range
		* between WaveLimits.x & WaveLimits.y. Then find a third point whose
		* distance from the second point is within the same range as well as being
		* closer to the starting point then the second point is.
		* This translates to a roughly back and forth motion */
		var firstIndex = _CurrentIndex;
		var secondIndex = -1;
		var thirdIndex = -1;

		/* get the averaged scaling of the player so that the movement distance
		* can be adjusted to account for scaling */ 
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
                var localDelta = Mathf.Abs(localPositions[firstIndex].x - localPositions[index].x);
                localDelta *= localAdjust;
                var worldBounded = WaveLimits.x <= delta && delta <= WaveLimits.y;
                var localBounded = WaveLimits.x <= localDelta && localDelta <= WaveLimits.y;
                if (worldBounded && localBounded) {
					secondIndex = index;
				}
			}
			else {
                var delta = Vector3.Distance(
                                                positions[secondIndex],
                                                positions[index]
                                            );
                var localDelta = Mathf.Abs(localPositions[secondIndex].x - localPositions[index].x);
				localDelta *= localAdjust;
                var worldBounded = WaveLimits.x <= delta && delta <= WaveLimits.y;
                var localBounded = WaveLimits.x <= localDelta && localDelta <= WaveLimits.y;
                if (worldBounded && localBounded)
                {
					var firstDelta = Vector3.Distance(
														positions[firstIndex],
														positions[index]
													);
                    var firstLocalDelta = Mathf.Abs(localPositions[firstIndex].x - localPositions[index].x);
                    firstLocalDelta *= localAdjust;
                    if ((firstDelta < delta && Mathf.Abs(firstDelta - delta) >= delta / 2) && 
                        (firstLocalDelta < localDelta && Mathf.Abs(firstLocalDelta - localDelta) >= localDelta / 2)) {
                        thirdIndex = index;
                        break;
					}
				}
			}
		}

		/* if the thirdIndex is no longer -1, then a thirdIndex was found
		* meaning a back and forth motion was found in the tracked history
		* of the hand */
        if (thirdIndex >= 0) {
        	Debug.Log("this should be a wave");
            LastWave = Time.deltaTime;
            return true;
        }
		return false;
	}

	void OnDrawGizmos() {
		/* draw spokes out from the center of the object to help make it
		* easier to see the hand angles */
		for (var i = 0; i < Divisions; i++) {
			var angle = i * Mathf.PI / (Divisions / 2);

			var target = Vector3.zero;
			target.y = 5 * Mathf.Cos(angle);
			target.x = 5 * Mathf.Sin(angle);

			var start = transform.position;
			target = transform.TransformPoint(target);

			UnityEngine.Gizmos.DrawLine(start, target);
		}

		/* draw sphers to indicate where the different rings for the hands are */
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

		/* draw rays for the palms of the hands. These help see the orientation
		* of the hand within the inspector */
		Gizmos.color = Color.red;
		Gizmos.DrawRay(LeftHand.position, LeftHand.up * -1);
		Gizmos.DrawRay(RightHand.position, RightHand.up * -1);
	}
}
