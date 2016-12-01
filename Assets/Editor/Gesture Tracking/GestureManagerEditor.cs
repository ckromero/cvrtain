using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GestureManager))]
public class GestureManagerEditor : Editor {

	private List<Gesture> _Gestures = new List<Gesture>(0);
	// private List<bool> _GestureVisible = new List<bool>(0);

	// private bool _ShowGestures = false;

	private bool _GesturesUpdated = false;

	public override void OnInspectorGUI() {

		_GesturesUpdated = false;

		var script = (GestureManager)target;

		serializedObject.Update();
		var property = serializedObject.FindProperty("Gestures");
		var gestureList = new List<Gesture>(script.Gestures);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("TestOutputText"));

		// while (gestureList.Count > _GestureVisible.Count) {
		// 	_GestureVisible.Add(true);
		// }

		property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Gestures");
		// _ShowGestures = EditorGUILayout.Foldout(_ShowGestures, "Gestures");
		// if (_ShowGestures) {
		if (property.isExpanded) {
			/* since this updates very very frequently, I only need to save
			* one value */
			var toRemove = -1;
			var toMoveUp = -1;
			var toMoveDown = -1;

			EditorGUI.indentLevel++;
			for (var i = 0; i < gestureList.Count; i++) {
				var name = gestureList[i].Name;
				name = (name == "") ? "New Gesture" : name;

				var gestureProperty = property.GetArrayElementAtIndex(i);
				var keyEnum = gestureProperty.FindPropertyRelative("ForceCompleteKey").enumNames;
				var keyIndex = gestureProperty.FindPropertyRelative("ForceCompleteKey").enumValueIndex;

				if (keyEnum[keyIndex] != "None") {
					var keyString = "(" + keyEnum[keyIndex] + ") ";
					name = string.Concat(keyString, name);
				}

				EditorGUILayout.BeginHorizontal();
				// var visible = _GestureVisible[i];
				var visible = property.GetArrayElementAtIndex(i).isExpanded;
				visible = EditorGUILayout.Foldout(visible, name);
				if (GUILayout.Button("Move Up", GUILayout.MaxWidth(75))) {
					toMoveUp = i;
				}
				if (GUILayout.Button("Move Down", GUILayout.MaxWidth(75))) {
					toMoveDown = i;
				}
				if (GUILayout.Button("X", GUILayout.MaxWidth(22))) {
					toRemove = i;
				}
				EditorGUILayout.EndHorizontal();

				if (visible) {
					// property.GetArrayElementAtIndex(i).isExpanded = visible;
					DisplayGesture(gestureList[i], property.GetArrayElementAtIndex(i));
				}
				property.GetArrayElementAtIndex(i).isExpanded = visible;
				// _GestureVisible[i] = visible;
			}
			serializedObject.ApplyModifiedProperties();

			/* handle shuffling the gestures around as needed */
			if (toRemove >= 0) {
				// _GestureVisible.RemoveAt(toRemove);
				gestureList.RemoveAt(toRemove);
			}
			if (toMoveUp >= 1) {
				// var visible = _GestureVisible[toMoveUp];
				// _GestureVisible.RemoveAt(toMoveUp);
				// _GestureVisible.Insert(toMoveUp - 1, visible);
				var gesture = gestureList[toMoveUp];
				gestureList.RemoveAt(toMoveUp);
				gestureList.Insert(toMoveUp - 1, gesture);				
			}
			if (toMoveDown >= 0 && toMoveDown < gestureList.Count - 1) {
				// var visible = _GestureVisible[toMoveDown];
				// _GestureVisible.RemoveAt(toMoveDown);
				// _GestureVisible.Insert(toMoveDown + 1, visible);
				var gesture = gestureList[toMoveDown];
				gestureList.RemoveAt(toMoveDown);
				gestureList.Insert(toMoveDown + 1, gesture);				
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.BeginHorizontal();
			/* other basic buttons for updating the list */
			if (GUILayout.Button("Add Gesture")) {
				// _GestureVisible.Add(true);
				var gesture = new Gesture();
				_Gestures.Add(gesture);
				gestureList.Add(new Gesture());
			}
			if (GUILayout.Button("Remove All")) {
				// _GestureVisible = new List<bool>(0);
				gestureList.Clear();
			}
			if (GUILayout.Button("Save Gestures To File")) {
				GestureCollection.WriteToGestureFile(gestureList.ToArray());	
			}
			EditorGUILayout.EndHorizontal();
		}

		script.Gestures = gestureList.ToArray();
		GestureCollection.Gestures = gestureList.ToArray();	
	}

	private void DisplayGesture(Gesture gesture, SerializedProperty gestureProperty) {

		var name = (gesture.Name == "") ? "New Gesture" : gesture.Name;
		// gesture.Name = EditorGUILayout.TextField("Name", name);

		EditorGUILayout.PropertyField(gestureProperty.FindPropertyRelative("Name"));
		EditorGUILayout.PropertyField(gestureProperty.FindPropertyRelative("ForceCompleteKey"));

		var ruleProperties = gestureProperty.FindPropertyRelative("Rules");

		var showRules = true;
		if (showRules) {
			EditorGUI.indentLevel++;
			/* since this updates very very frequently, I only need to save
			* one value */
			var toRemove = -1;
			var toMoveUp = -1;
			var toMoveDown = -1;

			var rulesList = new List<GestureRule>(gesture.Rules);

			for (var i = 0; i < rulesList.Count; i++) {
				var rule = rulesList[i];

				EditorGUILayout.BeginHorizontal();
				var ruleLabel = (i == 0) ? "Start Rule" : "Rule " + (i+1);
				EditorGUILayout.LabelField(ruleLabel);
				if (GUILayout.Button("Move Up", GUILayout.MaxWidth(75))) {
					toMoveUp = i;
				}
				if (GUILayout.Button("Move Down", GUILayout.MaxWidth(75))) {
					toMoveDown = i;
				}
				if (i >= 2 && GUILayout.Button("X", GUILayout.MaxWidth(20))) {
					toRemove = i;
				}
				EditorGUILayout.EndHorizontal();

				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(ruleProperties.GetArrayElementAtIndex(i));
				EditorGUI.indentLevel--;
			}

			/* handle shuffling the gestures around as needed */
			if (toRemove >= 0) {
				rulesList.RemoveAt(toRemove);
			}
			if (toMoveUp >= 1) {
				var rule = rulesList[toMoveUp];
				rulesList.RemoveAt(toMoveUp);
				rulesList.Insert(toMoveUp - 1, rule);				
			}
			if (toMoveDown >= 0 && toMoveDown < rulesList.Count - 1) {
				var rule = rulesList[toMoveDown];
				rulesList.RemoveAt(toMoveDown);
				rulesList.Insert(toMoveDown + 1, rule);				
			}
			EditorGUI.indentLevel--;
	    	GUILayout.BeginHorizontal();
	    	GUILayout.FlexibleSpace();
			if (GUILayout.Button("Add Rule", GUILayout.MaxWidth(100f))) {
				rulesList.Add(new GestureRule());
			}
	    	GUILayout.FlexibleSpace();
	    	GUILayout.EndHorizontal();
			/* other basic buttons for updating the list */


			gesture.Rules = rulesList.ToArray();
		}
	}
}
