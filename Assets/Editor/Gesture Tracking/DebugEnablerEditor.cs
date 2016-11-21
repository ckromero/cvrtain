using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugEnabler))]
public class DebugEnablersEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		var script = (DebugEnabler)target;
		var message = "Setup Debug";
		if (!Application.isPlaying) {
			message += " (only during Play mode)";
		}
		if (GUILayout.Button(message)) {
			if (Application.isPlaying) {
				script.SetupDebug();
			}
		}
	}
}