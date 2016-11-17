using UnityEngine;
using System.Collections;

public class LevelsManager : MonoBehaviour
{

	public ExpManager expManager;

	public AudioManager audioManager;

	public GestureManager gestureManager;

	public ClapManager clapManager;
	public bool IsIncrementStage = false;

	private bool listenToGestures = false;

	private  CompletedGestureStruct[] completedGestures;
	private  string lastGesture;
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
		if (IsIncrementStage) { 
			UpdateAV ();
			IsIncrementStage = false;
		}
	}

	void UpdateLevelsBasedOnGestures ()
	{
		//"Hands up bow","One Hand High, One Hand Low","Pump it up","Deep bow","Bow"
		//completedGestures

		if (lastGesture != gestureManager.LastGesture.Name) {
			
			if (gestureManager.LastGesture.Name == "Bow") {
				UpdateAV ();
				lastGesture = gestureManager.LastGesture.Name; 
			}
			if (gestureManager.LastGesture.Name == "Hands up bow") {
				UpdateAV ();
				lastGesture = gestureManager.LastGesture.Name; 
			}

		}
	}

	void UpdateAV ()
	{
		stage++;
		Debug.Log ("updating AV, stage is now" + stage);
		if (stage < audioPads.Length) {
			string newPad = audioPads [stage];
			audioManager.ChangePad (newPad);
			switch (stage) {
			case 1:
				clapManager.UpdateClappers("triggerGoToHigh") ;
				break;
			case 2:
				clapManager.UpdateClappers("triggerRaisedFist");
				break;
			case 3:
				clapManager.UpdateClappers("triggerHighClapping");
				break;
			default:
				break;
			}

//			animatorManager.changeAnimationState (newState);
//			lightsManager.changeLightState(newState);
		} else { 
			expManager.expState = ExpManager.ExpStates.Outro;
		}
	}

	public void StartLevels ()
	{ 
		listenToGestures = true;
	}

}
