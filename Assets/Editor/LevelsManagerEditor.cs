using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(LevelsManager))]
public class LevelsManagerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
	}
}
