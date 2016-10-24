using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GestureCallibrator))]
public class GestureCallibratorEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var script = (GestureCallibrator)target;
		var message = "Callibrate";
		if (!Application.isPlaying) {
			message += " (only during Play mode)";
		}
		if (GUILayout.Button(message)) {
			if (Application.isPlaying) {
				script.Callibrate();
			}
		}
	}	
}