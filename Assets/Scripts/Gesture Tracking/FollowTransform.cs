using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {

	public Transform[] Transforms;

	[HideInInspector]
	public bool XLock;
	[HideInInspector]
	public bool YLock;
	[HideInInspector]
	public bool ZLock;

	[HideInInspector]
	public bool XRotationLock;
	[HideInInspector]
	public bool YRotationLock;
	[HideInInspector]
	public bool ZRotationLock;

    private Quaternion _StartingRotation;

	// Use this for initialization
	void Start () {
        _StartingRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (Transforms.Length == 0) {
			return;
		}

		var position = Vector3.zero;
		foreach(var trans in Transforms) {
			position += trans.position;
		}	
		position /= Transforms.Length;

		if (XLock) {
			position.x = transform.position.x;
		}
		if (YLock) {
			position.y = transform.position.y;
		}
		if (ZLock) {
			position.z = transform.position.z;
		}
		transform.position = position;

		/* for rotation, just use the first object because otherwise the 
		* averaging results in some weird output very quickly */
		var rotation = Transforms[0].eulerAngles;

		if (XRotationLock) {
			rotation.x = transform.eulerAngles.x;
		}
		if (YRotationLock) {
			rotation.y = transform.eulerAngles.y;
		}
		if (ZRotationLock) {
			rotation.z = transform.eulerAngles.z;
		}
		transform.eulerAngles = _StartingRotation.eulerAngles + rotation;
	}
}
