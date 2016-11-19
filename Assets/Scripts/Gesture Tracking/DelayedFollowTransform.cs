using UnityEngine;
using System.Collections;

public class DelayedFollowTransform : MonoBehaviour {

	public float Delay;
	public Transform[] Transforms;

	[HideInInspector]
	public bool XLock;
	[HideInInspector]
	public bool YLock;
	[HideInInspector]
	public bool ZLock;

    private Quaternion _StartingRotation;

    private Vector3[] _TrackedPositions;
    private int _NewEntryIndex;

	// Use this for initialization
	void Start () {
        _StartingRotation = transform.rotation;
        _TrackedPositions = new Vector3[(int)Mathf.Floor(Delay * 60)];

        var position = AveragedPositions();
        for (var i = 0; i < _TrackedPositions.Length; i++) {
        	_TrackedPositions[i] = position;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Transforms.Length == 0) {
			return;
		}

		var position = AveragedPositions();

		_TrackedPositions[_NewEntryIndex] = position;

		_NewEntryIndex++;
		if (_NewEntryIndex >= _TrackedPositions.Length) _NewEntryIndex = 0;

		transform.position = _TrackedPositions[_NewEntryIndex];
	}

	private Vector3 AveragedPositions() {
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

		return position;
	}
}
