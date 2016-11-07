using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandsTracker))]
public class HandsTrackerEditor : Editor {

	void OnSceneGUI() {
		var t = target as HandsTracker;
		
		var leftLabel = "left hand\n" + t.LeftHandRing + " " + (int)t.LeftHandAngle;
		var rightLabel = "right hand\n" + t.RightHandRing + " " + (int)t.RightHandAngle;
		Handles.color = Color.cyan;
		Handles.Label(t.LeftHand.position, leftLabel);
		Handles.Label(t.RightHand.position, rightLabel);
	}

}
