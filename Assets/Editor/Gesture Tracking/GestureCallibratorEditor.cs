using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GestureCallibrator))]
public class GestureCallibratorEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var script = (GestureCallibrator)target;
		if (Application.isPlaying) {
			if (GUILayout.Button("Callibrate")) {
				script.Callibrate();
			}
		}
	}	
}