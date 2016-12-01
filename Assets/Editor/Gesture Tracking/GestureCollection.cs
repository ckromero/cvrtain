using UnityEngine;
using System;
using System.IO;

public class GestureCollection {

	private const string FILEPATH = "/Resources/Gestures.json";

	private static GestureCollection _instance;
	private static GestureCollection Instance {
		get {
			if (_instance == null) {
				_instance = new GestureCollection();
			}
			return _instance;
		}
	}

	private GestureCollection() {}

	private Gesture[] _Gestures;

	public static string[] Names {
		get {
			var names = new string[GestureCollection.Gestures.Length];
			for (var i = 0; i < names.Length; i++) {
				names[i] = GestureCollection.Gestures[i].Name;
			}
			return names;
		}
	}

	public static Gesture[] Gestures {
		get {
			if (GestureCollection.Instance._Gestures == null) {
				try {
					var path = Application.dataPath+FILEPATH;
					var text = File.ReadAllText(path);
					var collection = JsonUtility.FromJson<SerializedGestureCollection>(text);
					GestureCollection.Instance._Gestures = collection.Gestures;
				}
				catch (Exception e) {
					if (e is NullReferenceException) {
						Debug.Log("Gesture.json file is empty or corrupted");
					}
					else if (e is FileNotFoundException) {
						Debug.Log("Gesture.json file is missing");
					}
					return new Gesture[0];
				}
			}
			return GestureCollection.Instance._Gestures;
		}
		set {
			GestureCollection.Instance._Gestures = value;
			// var path = Application.dataPath+FILEPATH;
			// var collection = new SerializedGestureCollection(value);
			// File.WriteAllText(path, JsonUtility.ToJson(collection, true));
			// WriteToGestureFile(value);
		}
	}

	public static void WriteToGestureFile(Gesture[] gestures) {
		var path = Application.dataPath+FILEPATH;
		var collection = new SerializedGestureCollection(gestures);
		File.WriteAllText(path, JsonUtility.ToJson(collection, true));
	}
}

[System.Serializable]
class SerializedGestureCollection {
	public Gesture[] Gestures;

	public SerializedGestureCollection() {}	
	public SerializedGestureCollection(Gesture[] gestures) {
		Gestures = gestures;
	}	
}