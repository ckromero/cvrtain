using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(GestureManager))]
public class GestureManagerEditor : Editor {

	private List<Gesture> _Gestures = new List<Gesture>(0);
	private Object _Source = null;
	private List<bool> _GestureVisible = new List<bool>(0);

	private bool _ShowGestures = false;

	private bool _GesturesUpdated = false;

	public override void OnInspectorGUI() {

		_GesturesUpdated = false;

		var script = (GestureManager)target;

		_Source = EditorGUILayout.ObjectField("Debug Text", _Source, typeof(Text));
		if (_Source != null) {
			script.TestOutputText = (Text)_Source;
		}

		serializedObject.Update();
		var property = serializedObject.FindProperty("Gestures");
		var gestureList = new List<Gesture>(script.Gestures);

		while (gestureList.Count > _GestureVisible.Count) {
			_GestureVisible.Add(true);
		}

		_ShowGestures = EditorGUILayout.Foldout(_ShowGestures, "Gestures");
		if (_ShowGestures) {
			/* since this updates very very frequently, I only need to save
			* one value */
			var toRemove = -1;
			var toMoveUp = -1;
			var toMoveDown = -1;

			EditorGUI.indentLevel++;
			for (var i = 0; i < gestureList.Count; i++) {
				var name = gestureList[i].Name;
				name = (name == "") ? "New Gesture" : name;

				EditorGUILayout.BeginHorizontal();
				var visible = _GestureVisible[i];
				visible = EditorGUILayout.Foldout(visible, name);
				if (GUILayout.Button("Move Up", GUILayout.MaxWidth(75))) {
					toMoveUp = i;
				}
				if (GUILayout.Button("Move Down", GUILayout.MaxWidth(75))) {
					toMoveDown = i;
				}
				if (GUILayout.Button("Remove", GUILayout.MaxWidth(75))) {
					toRemove = i;
				}
				EditorGUILayout.EndHorizontal();

				if (visible) {
					DisplayGesture(gestureList[i], property.GetArrayElementAtIndex(i));
				}
				_GestureVisible[i] = visible;
			}
			serializedObject.ApplyModifiedProperties();

			/* handle shuffling the gestures around as needed */
			if (toRemove >= 0) {
				_GestureVisible.RemoveAt(toRemove);
				gestureList.RemoveAt(toRemove);
			}
			if (toMoveUp >= 1) {
				var visible = _GestureVisible[toMoveUp];
				_GestureVisible.RemoveAt(toMoveUp);
				_GestureVisible.Insert(toMoveUp - 1, visible);
				var gesture = gestureList[toMoveUp];
				gestureList.RemoveAt(toMoveUp);
				gestureList.Insert(toMoveUp - 1, gesture);				
			}
			if (toMoveDown >= 0 && toMoveDown < gestureList.Count - 1) {
				var visible = _GestureVisible[toMoveDown];
				_GestureVisible.RemoveAt(toMoveDown);
				_GestureVisible.Insert(toMoveDown + 1, visible);
				var gesture = gestureList[toMoveDown];
				gestureList.RemoveAt(toMoveDown);
				gestureList.Insert(toMoveDown + 1, gesture);				
			}
			EditorGUI.indentLevel--;

			/* other basic buttons for updating the list */
			if (GUILayout.Button("Add Gestures")) {
				_GestureVisible.Add(true);
				var gesture = new Gesture();
				_Gestures.Add(gesture);
				gestureList.Add(new Gesture());
			}
			if (GUILayout.Button("Remove All")) {
				_GestureVisible = new List<bool>(0);
				gestureList.Clear();
			}
		}

		script.Gestures = gestureList.ToArray();
	}

	private void DisplayGesture(Gesture gesture, SerializedProperty gestureProperty) {

		var name = (gesture.Name == "") ? "New Gesture" : gesture.Name;
		// gesture.Name = EditorGUILayout.TextField("Name", name);

		EditorGUILayout.PropertyField(gestureProperty.FindPropertyRelative("Name"));

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
				if (i >= 2 && GUILayout.Button("Remove", GUILayout.MaxWidth(75))) {
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

			/* other basic buttons for updating the list */
			if (GUILayout.Button("Add Rule", GUILayout.MaxWidth(100f))) {
				_GestureVisible.Add(true);
				rulesList.Add(new GestureRule());
			}

			gesture.Rules = rulesList.ToArray();
		}

		EditorGUI.indentLevel--;	
	}
}
