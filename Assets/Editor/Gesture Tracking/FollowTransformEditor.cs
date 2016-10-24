using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FollowTransform))]
public class FollowTransformEditor : Editor {

	public override void OnInspectorGUI() {

		DrawDefaultInspector();

		var script = (FollowTransform)target;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Movement Constraints: ", GUILayout.MaxWidth(200));
		script.XLock = EditorGUILayout.ToggleLeft("X", script.XLock, GUILayout.MaxWidth(25));
		script.YLock = EditorGUILayout.ToggleLeft("Y", script.YLock, GUILayout.MaxWidth(25));
		script.ZLock = EditorGUILayout.ToggleLeft("Z", script.ZLock, GUILayout.MaxWidth(25));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Rotation Constraints: ", GUILayout.MaxWidth(200));
		script.XRotationLock = EditorGUILayout.ToggleLeft("X", script.XRotationLock, GUILayout.MaxWidth(25));
		script.YRotationLock = EditorGUILayout.ToggleLeft("Y", script.YRotationLock, GUILayout.MaxWidth(25));
		script.ZRotationLock = EditorGUILayout.ToggleLeft("Z", script.ZRotationLock, GUILayout.MaxWidth(25));
		EditorGUILayout.EndHorizontal();

	}
	
}