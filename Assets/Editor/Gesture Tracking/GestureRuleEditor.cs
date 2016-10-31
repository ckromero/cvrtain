using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GestureRule))]
public class GestureRuleDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

    	var labelWidth = 200f;
    	var layoutOptions = new GUILayoutOption[]{};

		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("RequireHeadState"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var requireHead = property.FindPropertyRelative("RequireHeadState").boolValue;
		if (requireHead) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(property.FindPropertyRelative("HeadState"));
			EditorGUI.indentLevel--;
		}

		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("RequireHandAngles"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var requireAngle = property.FindPropertyRelative("RequireHandAngles").boolValue;
		if (requireAngle) {
			var vectorProperty = property.FindPropertyRelative("LeftHandAngles");
			GreaterThanLessThan(vectorProperty, "Left Hand");
			vectorProperty = property.FindPropertyRelative("RightHandAngles");
			GreaterThanLessThan(vectorProperty, "Right Hand");
		}
		
		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("RequireHandReach"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var requireReach = property.FindPropertyRelative("RequireHandReach").boolValue;
		if (requireReach) {
			var vectorProperty = property.FindPropertyRelative("LeftHandReach");
			GreaterThanLessThan(vectorProperty, "Left Hand");
			vectorProperty = property.FindPropertyRelative("RightHandReach");
			GreaterThanLessThan(vectorProperty, "Right Hand");
		}

		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("HasMaximumDuration"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var hasMaxDuration = property.FindPropertyRelative("HasMaximumDuration").boolValue;
		if (hasMaxDuration) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(property.FindPropertyRelative("MaxDuration"));
			EditorGUI.indentLevel--;
		}

		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("HasTimeLimit"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var hasTimeLimit = property.FindPropertyRelative("HasTimeLimit").boolValue;
		if (hasTimeLimit) {
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(property.FindPropertyRelative("TimeLimit"));
			EditorGUI.indentLevel--;
		}
	}

	private void GreaterThanLessThan(SerializedProperty vectorProperty, string name) {
		var x = (int)vectorProperty.FindPropertyRelative("x").floatValue;
		var y = (int)vectorProperty.FindPropertyRelative("y").floatValue;

		EditorGUI.indentLevel++;
		EditorGUILayout.BeginHorizontal();
		x = EditorGUILayout.IntField(x);
		EditorGUILayout.LabelField("<= " + name + " <=");
		y = EditorGUILayout.IntField(y);
		EditorGUILayout.EndHorizontal();
		EditorGUI.indentLevel--;

		vectorProperty.FindPropertyRelative("x").floatValue = x;
		vectorProperty.FindPropertyRelative("y").floatValue = y;
	}
}