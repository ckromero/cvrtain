using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GestureRule))]
public class GestureRuleDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUILayout.LabelField("I'm a rule!");
		// EditorGUI.PrefixLabel(position, label);
	}
}