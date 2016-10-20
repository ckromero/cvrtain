using UnityEngine;
using System.Collections;

public class HandsTracker : MonoBehaviour {

	public Transform LeftHand;
	public Transform RightHand;

	const int _DIVISIONS = 8;
	const int _RINGS = 3;
	const float _RING_RADIUS = 1.25f;

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
		var zone = (int)Mathf.Floor(angle / (Mathf.PI * 2 / _DIVISIONS));
		if (zone >= 0) zone++;
		return zone;
	}

	public int LeftHandRing {
		get {
			var distance = Vector3.Distance(LeftHand.position, transform.position);
			distance = Mathf.Clamp(distance/_RING_RADIUS, 0, _RINGS);
			return (int)Mathf.Floor(distance) + 1;
		}
	}

	public int RightHandRing {
		get {
			var distance = Vector3.Distance(RightHand.position, transform.position);
			distance = Mathf.Clamp(distance/_RING_RADIUS, 0, _RINGS);
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
		for (var i = 0; i < _DIVISIONS; i++) {
			var angle = i * Mathf.PI / (_DIVISIONS / 2);

			var target = Vector3.zero;
			target.y = 5 * Mathf.Cos(angle);
			target.z = 5 * Mathf.Sin(angle);

			var start = transform.position;
			target = transform.TransformPoint(target);

			UnityEngine.Gizmos.DrawLine(start, target);
		}

		for (var i = 1; i <= _RINGS; i++) {
			var color = Color.Lerp(Color.yellow, Color.magenta, (i-1f)/(_RINGS - 1f));
			Gizmos.color = color;
			Gizmos.DrawWireSphere(transform.position, i * _RING_RADIUS);
		}
	}
}
