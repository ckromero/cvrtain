using UnityEngine;
using System.Collections;

public class LevelsManager : MonoBehaviour
{

	public ExpManager expManager;

	public AudioManager audioManager;

	public GestureManager gestureManager;

	private bool listenToGestures = false;

	private  CompletedGestureStruct[] completedGestures;
	private  CompletedGestureStruct lastGesture;
	private int stage = 0;
	private string[] audioPads = { "polite", "medium", "large", "huge" };

	//Listen to ExpManager

	//Listen to GestureManager

	//Manage LevelController

	//Update AnimatorController

	//Update AudioController

	//Update LightsController

	//Update ExpManager

	// Use this for initialization
	void Start ()
	{
		if (listenToGestures)
			UpdateLevelsBasedOnGestures ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (listenToGestures) {
			UpdateLevelsBasedOnGestures ();
		}
	}

	void UpdateLevelsBasedOnGestures ()
	{
		//"Hands up bow","One Hand High, One Hand Low","Pump it up","Deep bow","Bow"
//		completedGestures

		// if (lastGesture != gestureManager.LastGesture) {
			
		// 	if (gestureManager.LastGesture.Name == "Bow") {
		// 		UpdateAV ();
		// 		lastGesture = gestureManager.LastGesture; 
		// 	}
		// 	if (gestureManager.LastGesture.Name == "Hands up bow") {
		// 		UpdateAV ();
		// 		lastGesture = gestureManager.LastGesture; 
		// 	}
		// }

	}

	void UpdateAV ()
	{

		stage++;
		if (stage <= audioPads.Length) {
			string newPad = audioPads [stage];
			audioManager.ChangePad (newPad);
//			animatorManager.changeAnimationState (newState);
//			lightsManager.changeLightState(newState);
		} else { 
			expManager.expState = ExpManager.ExpStates.CurtainClose;
		}
	}

	public void StartLevels ()
	{ 
		listenToGestures = true;
	}

}
