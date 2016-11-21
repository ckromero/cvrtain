using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DelayedFollowTransform))]
public class DelayedFollowTransformEditor : Editor {

	public override void OnInspectorGUI() {

		DrawDefaultInspector();

		var script = (DelayedFollowTransform)target;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Movement Constraints: ", GUILayout.MaxWidth(200));
		script.XLock = EditorGUILayout.ToggleLeft("X", script.XLock, GUILayout.MaxWidth(25));
		script.YLock = EditorGUILayout.ToggleLeft("Y", script.YLock, GUILayout.MaxWidth(25));
		script.ZLock = EditorGUILayout.ToggleLeft("Z", script.ZLock, GUILayout.MaxWidth(25));
		EditorGUILayout.EndHorizontal();
	}
	
}