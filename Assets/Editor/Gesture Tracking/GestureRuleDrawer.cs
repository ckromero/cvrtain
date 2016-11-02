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
			EditorGUI.indentLevel++;
			var content = new GUIContent("Use Either Angle");
			EditorGUILayout.PropertyField(property.FindPropertyRelative("EitherHandAngle"), content, layoutOptions);
			var useEither = property.FindPropertyRelative("EitherHandAngle").boolValue;
			var message1 = "Left Hand";
			var message2 = "Right Hand";
			if (useEither) {
				message1 = "One Hand";
				message2 = "Other Hand";
			}
			var maxMinProperty = property.FindPropertyRelative("LeftHandAngles");
			MaxMin(maxMinProperty, message1);
			maxMinProperty = property.FindPropertyRelative("RightHandAngles");
			MaxMin(maxMinProperty, message2);
			EditorGUI.indentLevel--;
		}
		
		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUILayout.PropertyField(property.FindPropertyRelative("RequireHandReach"), layoutOptions);
		EditorGUIUtility.labelWidth = 0f;
		var requireReach = property.FindPropertyRelative("RequireHandReach").boolValue;
		if (requireReach) {
			EditorGUI.indentLevel++;
			var content = new GUIContent("Use Either Reach");
			EditorGUILayout.PropertyField(property.FindPropertyRelative("EitherHandReach"), content, layoutOptions);

			var useEither = property.FindPropertyRelative("EitherHandReach").boolValue;
			var message1 = "Left Hand";
			var message2 = "Right Hand";
			if (useEither) {
				message1 = "One Hand";
				message2 = "Other Hand";
			}
			var maxMinProperty = property.FindPropertyRelative("LeftHandReach");
			MaxMin(maxMinProperty, message1);
			maxMinProperty = property.FindPropertyRelative("RightHandReach");
			MaxMin(maxMinProperty, message2);
			EditorGUI.indentLevel--;
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

		EditorGUILayout.BeginHorizontal();
		x = EditorGUILayout.IntField(x);
		EditorGUILayout.LabelField("<= " + name + " <=");
		y = EditorGUILayout.IntField(y);
		EditorGUILayout.EndHorizontal();

		vectorProperty.FindPropertyRelative("x").floatValue = x;
		vectorProperty.FindPropertyRelative("y").floatValue = y;
	}

	private void MaxMin(SerializedProperty maxMinProperty, string name) {
		var max = (int)maxMinProperty.FindPropertyRelative("Max").floatValue;
		var min = (int)maxMinProperty.FindPropertyRelative("Min").floatValue;

		EditorGUILayout.BeginHorizontal();
		min = EditorGUILayout.IntField(min);
		EditorGUILayout.LabelField("<= " + name + " <=");
		max = EditorGUILayout.IntField(max);
		EditorGUILayout.EndHorizontal();

		maxMinProperty.FindPropertyRelative("Max").floatValue = max;
		maxMinProperty.FindPropertyRelative("Min").floatValue = min;
	}
}