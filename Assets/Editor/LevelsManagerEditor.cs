using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelsManager))]
public class LevelsManagerEditor : Editor {

	public override void OnInspectorGUI() {
		// DrawDefaultInspector()

		serializedObject.Update();
		var propertyIterator = serializedObject.GetIterator();

		var next = propertyIterator.NextVisible(true);
		while (next) {
			if (propertyIterator.isArray) {
				/* the only way I could come up with verifying the type of the
				* serialized property. Perhaps I'm going about this all wrong */
				var levelType = typeof(Level) + "";
				if (propertyIterator.type == levelType) {
					DisplayLevels(propertyIterator);
				}
			}
			else {
				EditorGUILayout.PropertyField(propertyIterator);
			}
			next = propertyIterator.NextVisible(false);
		}

		serializedObject.ApplyModifiedProperties();
	}

	void DisplayLevels(SerializedProperty levels) {
		var arrayLength = levels.arraySize;

		levels.isExpanded = EditorGUILayout.Foldout(levels.isExpanded, levels.name);
		if (!levels.isExpanded) {
			return;
		}

		var removalIndex = -1;
		var moveUpIndex = -1;
		var moveDownIndex = -1;
		var startFound = false;

		for (var i = 0; i < arrayLength; i++) {
			var level = levels.GetArrayElementAtIndex(i);
			EditorGUI.indentLevel++;
			var name = "Level " + i;
			if (level.FindPropertyRelative("StartingLevel").boolValue && !startFound) {
				name += " (Starting Level)";
				startFound = true;
			}
			GUILayout.BeginHorizontal();
			level.isExpanded = EditorGUILayout.Foldout(level.isExpanded, name);
			if (GUILayout.Button("Move Up", GUILayout.MaxWidth(80f))) {
				moveUpIndex = i;
			}
			if (GUILayout.Button("Move Down", GUILayout.MaxWidth(80f))) {
				moveDownIndex = i;
			}
			if (GUILayout.Button("X", GUILayout.MaxWidth(20f))) {
				removalIndex = i;
			}
			GUILayout.EndHorizontal();
			if (level.isExpanded) {
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(level);
				EditorGUILayout.LabelField("");
				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}

    	GUILayout.BeginHorizontal();
    	GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add Level", GUILayout.MaxWidth(100f))) {
			levels.InsertArrayElementAtIndex(arrayLength);
		}
    	GUILayout.FlexibleSpace();
    	GUILayout.EndHorizontal();

    	if (removalIndex >= 0) {
    		levels.DeleteArrayElementAtIndex(removalIndex);
    	}
    	else if (moveUpIndex >= 1) {
    		levels.MoveArrayElement(moveUpIndex, moveUpIndex-1);
    	}
    	else if (moveDownIndex >= 0 && moveUpIndex < levels.arraySize - 2) {
    		levels.MoveArrayElement(moveDownIndex, moveDownIndex+1);
    	}
	}
}
