using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Gesture))]
public class GestureDrawer : PropertyDrawer {

	float _Height = 18f;
	bool _FoldOut = false;
	
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		// EditorGUI.indentLevel++;
		// position.x += 5f;
		// var content = new GUIContent("Name");
		// var style = EditorStyles.textField;
		// EditorGUILayout.PropertyField(property.FindPropertyRelative("Name"));
		// EditorGUILayout.LabelField("Rules");

		// var showRules = true;
		// if (showRules) {
		// 	/* since this updates very very frequently, I only need to save
		// 	* one value */
		// 	var toRemove = -1;
		// 	var toMoveUp = -1;
		// 	var toMoveDown = -1;

		// 	var rules = property.FindPropertyRelative("Rules");

		// 	EditorGUI.indentLevel++;
		// 	for (var i = 0; i < rules.arraySize; i++) {
		// 		var ruleProperty = rules.GetArrayElementAtIndex(i);

		// 		EditorGUILayout.BeginHorizontal();
		// 		if (GUILayout.Button("Move Up", GUILayout.MaxWidth(75))) {
		// 			toMoveUp = i;
		// 		}
		// 		if (GUILayout.Button("Move Down", GUILayout.MaxWidth(75))) {
		// 			toMoveDown = i;
		// 		}
		// 		if (GUILayout.Button("Remove", GUILayout.MaxWidth(75))) {
		// 			toRemove = i;
		// 		}
		// 		EditorGUILayout.EndHorizontal();

		// 		EditorGUILayout.PropertyField(rule);
		// 	}

		// 	/* handle shuffling the gestures around as needed */
		// 	if (toRemove >= 0) {
		// 		_GestureVisible.RemoveAt(toRemove);
		// 		gestureList.RemoveAt(toRemove);
		// 	}
		// 	if (toMoveUp >= 1) {
		// 		var visible = _GestureVisible[toMoveUp];
		// 		_GestureVisible.RemoveAt(toMoveUp);
		// 		_GestureVisible.Insert(toMoveUp - 1, visible);
		// 		var gesture = gestureList[toMoveUp];
		// 		gestureList.RemoveAt(toMoveUp);
		// 		gestureList.Insert(toMoveUp - 1, gesture);				
		// 	}
		// 	if (toMoveDown >= 0 && toMoveDown < _TestingList.Count - 1) {
		// 		var visible = _GestureVisible[toMoveDown];
		// 		_GestureVisible.RemoveAt(toMoveDown);
		// 		_GestureVisible.Insert(toMoveDown + 1, visible);
		// 		var gesture = gestureList[toMoveDown];
		// 		gestureList.RemoveAt(toMoveDown);
		// 		gestureList.Insert(toMoveDown + 1, gesture);				
		// 	}
		// 	EditorGUI.indentLevel--;

		// 	/* other basic buttons for updating the list */
		// 	if (GUILayout.Button("Add Rule")) {
		// 		_GestureVisible.Add(true);
		// 		var gesture = new Gesture();
		// 		_Gestures.Add(gesture);
		// 		gestureList.Add(new Gesture());
		// 	}

		// 	script.Gestures = gestureList.ToArray();
		// }

		// EditorGUI.indentLevel--;
	}
}