using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OneWayCollider : MonoBehaviour {

	private const float UPPER_COLLIDER_PERCENT = 0.1f;

	public Vector3 Center = Vector3.zero;
	public Vector3 Size = Vector3.one;

	public Vector3 UpperColliderCenter {
		get {
			var center = Center;
			center.y += Size.y * ((1f - UPPER_COLLIDER_PERCENT) / 2);
			return center;
		}
	}

	public Vector3 UpperColliderSize {
		get {
			var size = Size;
			size.y *= UPPER_COLLIDER_PERCENT;
			return size;
		}
	}

	public Vector3 LowerColliderCenter {
		get {
			var center = Center;
			center.y -= Size.y * (UPPER_COLLIDER_PERCENT / 2);
			return center;
		}
	}

	public Vector3 LowerColliderSize {
		get {
			var size = Size;
			size.y *= 1f - UPPER_COLLIDER_PERCENT;
			return size;
		}
	}

	private List<Collider> _LowerOverlappingColliders;
	private List<Collider> _UpperOverlappingColliders;
	private List<Collider> _ViableColliders;

	void Awake() {
		_LowerOverlappingColliders = new List<Collider>();
		_UpperOverlappingColliders = new List<Collider>();
		_ViableColliders = new List<Collider>();

		var upperObject = new GameObject();
		upperObject.transform.parent = transform;
		upperObject.name = "Upper Collider";
		upperObject.transform.localPosition = UpperColliderCenter;
		upperObject.transform.localRotation = Quaternion.identity;
		upperObject.layer = gameObject.layer;
		var upperRigidbody = (Rigidbody)upperObject.AddComponent<Rigidbody>();
		upperRigidbody.isKinematic = true;
		var upperCollider = (BoxCollider)upperObject.AddComponent<BoxCollider>();
		upperCollider.size = UpperColliderSize;
		upperCollider.isTrigger = true;
		var subscriber = (ColliderSubscriber)upperObject.AddComponent<ColliderSubscriber>();
		subscriber.OnTriggerEnterDelegate += OnUpperTriggerEnter;
		subscriber.OnTriggerExitDelegate += OnUpperTriggerExit;

		var lowerObject = new GameObject();
		lowerObject.transform.parent = transform;
		lowerObject.name = "Lower Collider";
		lowerObject.transform.localPosition = LowerColliderCenter;
		lowerObject.transform.localRotation = Quaternion.identity;
		lowerObject.layer = gameObject.layer;
		var lowerRigidbody = (Rigidbody)lowerObject.AddComponent<Rigidbody>();
		lowerRigidbody.isKinematic = true;
		var lowerCollider = (BoxCollider)lowerObject.AddComponent<BoxCollider>();
		lowerCollider.size = LowerColliderSize;
		lowerCollider.isTrigger = true;
		subscriber = (ColliderSubscriber)lowerObject.AddComponent<ColliderSubscriber>();
		subscriber.OnTriggerEnterDelegate += OnLowerTriggerEnter;
		subscriber.OnTriggerExitDelegate += OnLowerTriggerExit;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		var rotation = transform.rotation;
		var oldMatrix = Gizmos.matrix;

		/* the transforming of the Gizmo matrix is needed to allow the object
		* to be rotated and have the gizmos correctly reflect that */
		Gizmos.color = Color.blue;
		var center = transform.TransformPoint(UpperColliderCenter);
		var cubeMatrix = Matrix4x4.TRS(center, rotation, transform.lossyScale);
		Gizmos.matrix *= cubeMatrix;
		Gizmos.DrawWireCube(Vector3.zero, UpperColliderSize);

		Gizmos.color = Color.green;
		center = transform.TransformPoint(LowerColliderCenter);
		cubeMatrix = Matrix4x4.TRS(center, rotation, transform.lossyScale);
		Gizmos.matrix = oldMatrix * cubeMatrix;
		Gizmos.DrawWireCube(Vector3.zero, LowerColliderSize);

		Gizmos.matrix = oldMatrix;
	}

	void OnUpperTriggerEnter(Collider other) {
		_UpperOverlappingColliders.Add(other);

		if (!_LowerOverlappingColliders.Contains(other)) {
			_ViableColliders.Add(other);
			OnTriggerEnter(other);
		}
	}

	void OnLowerTriggerEnter(Collider other) {
		_LowerOverlappingColliders.Add(other);	
	}

	void OnUpperTriggerExit(Collider other) {
		_UpperOverlappingColliders.Remove(other);

		if (!_LowerOverlappingColliders.Contains(other)
				&& _ViableColliders.Contains(other)) {
			_ViableColliders.Remove(other);
			OnTriggerExit(other);
		}
	}

	void OnLowerTriggerExit(Collider other) {

		_LowerOverlappingColliders.Remove(other);

		if (!_UpperOverlappingColliders.Contains(other)
				&& _ViableColliders.Contains(other)) {
			_ViableColliders.Remove(other);
			OnTriggerExit(other);
		}
	}

	public delegate void onTriggerEnter(Collider other);
	public event onTriggerEnter OnTriggerEnter;

	public delegate void onTriggerExit(Collider other);
	public event onTriggerExit OnTriggerExit;
}
