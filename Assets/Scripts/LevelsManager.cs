using UnityEngine;
using UnityEngine.Assertions;
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
		var message = "Invalid gesture";
		foreach (var level in Levels) {
			Assert.IsTrue(gestureManager.CompareGestureNames(level.PositiveGestures), message);
			Assert.IsTrue(gestureManager.CompareGestureNames(level.NeutralGestures), message);
			Assert.IsTrue(gestureManager.CompareGestureNames(level.NegativeGestures), message);
		}
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
			
			// if (gestureManager.LastGesture.Name == "Bow") {
			// 	UpdateAV ();
			// 	lastGesture = gestureManager.LastGesture.Name; 
			// }
			// if (gestureManager.LastGesture.Name == "Hands up bow") {
			// 	UpdateAV ();
			// 	lastGesture = gestureManager.LastGesture.Name; 
			// }

			// lastGesture = gestureManager.LastGesture.Name;
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
			levelCompletion += evaluation;

			var promotionReq = Levels[stage].PromotionReq;
			var demotionReq = Levels[stage].DemotionReq;
			if (levelCompletion >= promotionReq) {
				stage = Mathf.Clamp(++stage, 0, Levels.Length - 1);
				UpdateLevel();
			}
			else if (levelCompletion < 0 && Mathf.Abs(levelCompletion) >= demotionReq) {
				stage = Mathf.Clamp(--stage, 0, Levels.Length - 1);
				UpdateLevel();
			}
		}
	}

	void UpdateLevel() {
		levelCompletion = 0;
		UpdateAV();
	}

	void UpdateAV ()
	{
		// stage++;
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

[System.Serializable]
public struct Level {
	public string[] PositiveGestures;
	public string[] NeutralGestures;
	public string[] NegativeGestures;

	public int PromotionReq;
	public int DemotionReq;

	public int EvaluateGesture(string gesture) {
		foreach(var name in PositiveGestures) {
			if (name == gesture) {
				return 1;
			}
		}
		foreach(var name in NeutralGestures) {
			if (name == gesture) {
				return 0;
			}
		}
		foreach(var name in NegativeGestures) {
			if (name == gesture) {
				return -1;
			}
		}

		/* return NaN if the gesture is not contained in this array */
		return -100000;
	}
}
