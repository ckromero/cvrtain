using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;

public class LevelsManager : MonoBehaviour
{

	public ExpManager expManager;

	public AudioManager audioManager;

	public GestureManager gestureManager;

	public ClapManager clapManager;
	public bool IsIncrementStage = false;

	private bool listenToGestures = true;

	private  CompletedGestureStruct[] completedGestures;
	// private  string lastGesture;
	private CompletedGestureStruct lastGesture;
	private int stage = 0;
	private string[] audioPads = { "polite", "medium", "large", "huge" };

	[SerializeField]
	private Level[] Levels;
	private int levelIndex = 0;
	private int levelCompletion;

	public bool MaxLevel { get; private set; }

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

		/* double check that all gesture names entered are actually valid gestures */
		// var message = "Invalid gesture";
		// foreach (var level in Levels) {
		// 	Assert.IsTrue(gestureManager.CompareGestureNames(level.PositiveGestures), message);
		// 	Assert.IsTrue(gestureManager.CompareGestureNames(level.NeutralGestures), message);
		// 	Assert.IsTrue(gestureManager.CompareGestureNames(level.NegativeGestures), message);
		// }

		UpdateAV();

		for (var i = 0; i < Levels.Length; i++) {
			if (Levels[i].StartingLevel) {
				stage = i;
				break;
			}
		}
		Levels[stage].Reset();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (listenToGestures) {
			UpdateLevelsBasedOnGestures ();
		}
		if (IsIncrementStage) { 
			stage++;
			UpdateAV ();
			IsIncrementStage = false;
		}
	}

	void UpdateLevelsBasedOnGestures ()
	{
		//"Hands up bow","One Hand High, One Hand Low","Pump it up","Deep bow","Bow"
		//completedGestures

		if (lastGesture.Time != gestureManager.LastGesture.Time) {
			
			lastGesture = gestureManager.LastGesture;
			var evaluation = Levels[stage].EvaluateGesture(lastGesture.Name);

			switch (evaluation) {
				case 1: 
					Debug.Log("a good thing");
					break;
				case 0:
					Debug.Log("a neutral thing");
					break;
				case -1:
					Debug.Log("a bad thing");
					break;
				default:
					return;
			}

			if (Levels[stage].Complete) {
				if (stage == Levels.Length - 1) {
					MaxLevel = true;
				}
				stage = Mathf.Clamp(++stage, 0, Levels.Length - 1);
				UpdateLevel();
			}
			else if (Levels[stage].Failed) {
				stage = Mathf.Clamp(--stage, 0, Levels.Length - 1);
				UpdateLevel();
			}
		}
	}

	void UpdateLevel() {
		levelCompletion = 0;
		Levels[stage].Reset();
		UpdateAV();
	}

	void UpdateAV ()
	{
		// stage++;
		Debug.Log ("updating AV, stage is now " + stage);
		if (stage < audioPads.Length) {
			string newPad = audioPads [stage];
			audioManager.ChangePad (newPad);
			switch (stage) {
			case 0:
				clapManager.UpdateClappers("triggerGoToHigh") ;
				break;
			case 1:
				clapManager.UpdateClappers("triggerRaisedFist");
				break;
			case 2:
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

[System.Serializable]
public class Level {

	[System.Serializable]
	private struct GestureTracker {
		public string Gesture;
		public int Limit;
		public int RemainingLimit;
	}

	public bool StartingLevel;

	[SerializeField]
	private GestureTracker[] PositiveGestures;
	[SerializeField]
	private GestureTracker[] NeutralGestures;
	[SerializeField]
	private GestureTracker[] NegativeGestures;

	public int PromotionRequirment;
	public int DemotionRequirment;

	private int _LevelStatus = 0;
	public bool Complete {
		get {return (_LevelStatus >= PromotionRequirment);}
	}
	public bool Failed {
		get {return (_LevelStatus <= DemotionRequirment * -1);}
	}

	public Level() {}

	public int EvaluateGesture(string gesture) {
		for (var i = 0; i < PositiveGestures.Length; i++) {
			if (PositiveGestures[i].Gesture == gesture) {
				if (PositiveGestures[i].RemainingLimit > 0) {
					PositiveGestures[i].RemainingLimit--;
					_LevelStatus++;
					return 1;
				}	
				return -100000;
			}
		}
		for (var i = 0; i < NeutralGestures.Length; i++) {
			if (NeutralGestures[i].Gesture == gesture) {
				if (NeutralGestures[i].RemainingLimit > 0) {
					NeutralGestures[i].RemainingLimit--;
					return 0;
				}	
				return -100000;
			}
		}
		for (var i = 0; i < NegativeGestures.Length; i++) {
			if (NegativeGestures[i].Gesture == gesture) {
				if (NegativeGestures[i].RemainingLimit > 0) {
					NegativeGestures[i].RemainingLimit--;
					_LevelStatus--;
					return -1;
				}	
				return -100000;
			}
		}
		/* massive negative to indicate the gesture is not available */
		return -100000;
	}

	public void Reset() {
		for (var i = 0; i < PositiveGestures.Length; i++) {
			PositiveGestures[i].RemainingLimit = PositiveGestures[i].Limit;
		}
		for (var i = 0; i < NeutralGestures.Length; i++) {
			NeutralGestures[i].RemainingLimit = NeutralGestures[i].Limit;
		}
		for (var i = 0; i < NegativeGestures.Length; i++) {
			NegativeGestures[i].RemainingLimit = NegativeGestures[i].Limit;
		}
		_LevelStatus = 0;
	}
}
