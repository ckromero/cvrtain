using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Level))]
public class LevelDrawer : PropertyDrawer {

    public override void OnGUI(Rect position,
    							SerializedProperty property,
    							GUIContent label) {

    	EditorGUI.indentLevel++;
    	property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Level");
    	if (!property.isExpanded) {
    		EditorGUI.indentLevel--;
    		return;
    	}

    	label = EditorGUI.BeginProperty(position, label, property);
    	var layoutOptions = new GUILayoutOption[]{};

    	EditorGUI.indentLevel++;

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

    	EditorGUI.indentLevel-=2;
    	EditorGUI.EndProperty();
    }

    private string[] DisplayGestureGroup(SerializedProperty property) {
    	// EditorGUI.LabelField(position, property.name);
    	// EditorGUILayout.LabelField(property.name);
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
    	if (gestureNames.Length == 0) {
    		gestureNames = new string[1]{property.stringValue};
    	}
    	var selectedIndex = -1;
    	for (var i = 0; i < gestureNames.Length; i++) {
    		if (property.stringValue == gestureNames[i])	 {
    			selectedIndex = i;
    			break;
    		}
    	}	
    	selectedIndex = EditorGUILayout.Popup(selectedIndex, gestureNames);
    	if (GUILayout.Button("-", EditorStyles.miniButtonRight)) {
    		return false;
    	}
    	EditorGUILayout.EndHorizontal();
    	if (EditorGUI.EndChangeCheck() && selectedIndex >= 0) {
    		property.stringValue = gestureNames[selectedIndex];
    	}
    	return true;
    }
}
