using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(Gesture))]
public class GestureEditor : Editor {

	public override void OnInspectorGUI() {
		EditorGUILayout.LabelField("HELLO WORLD");
		Debug.Log("is this running though?");
	}
}