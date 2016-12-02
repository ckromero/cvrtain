using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Level))]
public class LevelDrawer : PropertyDrawer {

    public override void OnGUI(Rect position,
    							SerializedProperty property,
    							GUIContent label) {


    	label = EditorGUI.BeginProperty(position, label, property);

    	var next = property.Next(true);
    	var gestureDispalyCount = 0;
    	while (next) {
    		if (property.isArray) {
    			DisplayGestureGroup(property);
	    	}
	    	else if (!property.hasVisibleChildren) {
	    		EditorGUILayout.PropertyField(property);
	    	}
    		next = property.Next(false);
    	}

    	EditorGUI.EndProperty();
    }

    private string[] DisplayGestureGroup(SerializedProperty property) {
    	property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.name);
    	if (!property.isExpanded) {
    		return new string[0];
    	}
    	EditorGUI.indentLevel++;
    	var setNames = new string[property.arraySize];
    	var removalIndex = -1;
    	for (var i = 0; i < property.arraySize; i++) {
    		if (!DisplayGesture(property.GetArrayElementAtIndex(i))) {
    			removalIndex = i;
    		}
    	}
    	GUILayout.BeginHorizontal();
    	GUILayout.FlexibleSpace();
    	if (GUILayout.Button("+")) {
			property.InsertArrayElementAtIndex(property.arraySize);
            var newProperty = property.GetArrayElementAtIndex(property.arraySize - 1);
            newProperty.FindPropertyRelative("Limit").intValue = 100;
    		EditorGUI.indentLevel--;
    		return setNames;
    	}
    	GUILayout.FlexibleSpace();
    	GUILayout.EndHorizontal();
    	if (removalIndex >= 0) {
    		property.DeleteArrayElementAtIndex(removalIndex);
    	}
    	EditorGUI.indentLevel--;
    	return setNames;
    }

    private bool DisplayGesture(SerializedProperty property) {
    	EditorGUI.BeginChangeCheck();
    	EditorGUILayout.BeginHorizontal();
    	var gestureNames = GestureCollection.Names;
        var selectedName = property.FindPropertyRelative("Gesture").stringValue;
    	if (gestureNames.Length == 0) {
    		gestureNames = new string[1]{selectedName};
    	}
    	var selectedIndex = -1;
    	for (var i = 0; i < gestureNames.Length; i++) {
    		if (selectedName == gestureNames[i]) {
    			selectedIndex = i;
    			break;
    		}
    	}	
    	selectedIndex = EditorGUILayout.Popup(selectedIndex, gestureNames);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Limit"));
    	if (GUILayout.Button("-", EditorStyles.miniButtonRight)) {
    		return false;
    	}
    	EditorGUILayout.EndHorizontal();
    	if (EditorGUI.EndChangeCheck() && selectedIndex >= 0) {
    		property.FindPropertyRelative("Gesture").stringValue = gestureNames[selectedIndex];
    	}
    	return true;
    }
}
