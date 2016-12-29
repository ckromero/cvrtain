using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelsManager : MonoBehaviour
{
	public ExpManager expManager;
	public AudioManager audioManager;
	public GestureManager gestureManager;
	public ClapManager clapManager;

	public float DelayBeforeDecayStarts = 10f;
	public float DecayGap = 2f;
	public int CurrentLevel { get{ return stage; }}
	//TODO: @owenbell not sure if this needs public, get/set or if we even really need it all?
	public bool Performing { get; set; }
	public bool IsIncrementStage = false;
	public bool MaxLevel { get; private set; }

	// private  string lastGesture;
	private CompletedGestureStruct lastGesture;
	private CompletedGestureStruct[] completedGestures;

	[SerializeField]
	private Level[] Levels;

	//TODO: @ckromero brittle here, needs a shared enumeration
	//private string[] audioPads = { "murmur", "allQuiet", "polite", "medium", "large", "huge" };
	private float _TimeSinceLastGesture = 0f;
	private int stage = 0;
	private int _HighestStage;

	private int levelIndex = 0;
	private int levelCompletion;
	private int stateLevelIndex = 0;
	private bool listenToGestures = true;

	void Awake() {
		Performing = false;
	}

	void Start () {
		// if (listenToGestures)
		// 	UpdateLevelsBasedOnGestures ();
		//TODO: make this a function that is called at start AND at reset.
		// for (var i = 0; i < Levels.Length; i++) {
		// 	if (Levels[i].StartingLevel) {
		// 		stage = i;
		// 		stateLevelIndex = i;
		// 		_HighestStage = stage;
		// 		break;
		// 	}
		// }
		// Levels[stage].Reset();

		// Reset();

		// UpdateAV();
		//TODO: make this a cue in experience manager
		// audioManager.ChangePad("murmur");
	}

	public void BeginPerforming() {
		Debug.Log("BEGIN THE PERFORMANCE!!!!!");
		UpdateAV();		
		gestureManager.Tracking = true;
		Performing = true;
		Reset();
        listenToGestures = true;
		//TODO: expManager should be queued to audience start state	
	}

	public void StopPerforming () { 
		//gestureManager.Reset();
        gestureManager.Tracking = false;
		Performing = false;
	}

	public void Reset() {
		for (var i = 0; i < Levels.Length; i++) {
			if (Levels[i].StartingLevel) {
				stage = i;
				stateLevelIndex = i;
				_HighestStage = stage;
				break;
			}
		}
		Levels[stage].Reset();
	}

	// Update is called once per frame
	void Update ()
	{
		if (listenToGestures && Performing) {
			UpdateLevelsBasedOnGestures ();
		}
		//TODO: remove?
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

		if (!lastGesture.Time.Equals(gestureManager.LastGesture.Time)) {

			_TimeSinceLastGesture = 0f;
			lastGesture = gestureManager.LastGesture;

			if (gestureManager.Facing == HeadFacing.Back) {
				/* only trigger a response if the player is past the starting
				* state */
				if (stage <= stateLevelIndex) {
					return;
				}
				Debug.Log("the player is being inappropriate");
				//TODO: sendcue on experience manager
				audioManager.TriggerSound("laugh");
				return;
			}

			var evaluation = Levels[stage].EvaluateGesture(lastGesture.Name);
			if (_HighestStage == Levels.Length - 1 && _HighestStage != stage) {
				var highEvaluation = Levels[_HighestStage].EvaluateGesture(lastGesture.Name);
				if (highEvaluation > -1) {
					SetLevelTo(_HighestStage);
					return;
				}
			}

		}
		//TODO: change to weird dance or weird random gesture.
		else if (!gestureManager.WeirdRandomMovement) {
			_TimeSinceLastGesture += Time.deltaTime;
			if (_TimeSinceLastGesture > DelayBeforeDecayStarts) {
				var decayTime = _TimeSinceLastGesture - DelayBeforeDecayStarts;
				if (decayTime > DecayGap) {
					_TimeSinceLastGesture -= DecayGap;
					Levels[stage].Decrement();
				}
			}
		}

		if (Levels[stage].Complete) {
			//TODO: nothing below polite applause
            // stage += Levels[stage].Advancement;
            // stage = Mathf.Clamp(stage, 0, Levels.Length - 1);
            // UpdateLevel();
            Debug.Log("advance stage");
			UpdateLevel(Levels[stage].Advancement);
		}
		else if (Levels[stage].Failed) {
			// stage = Mathf.Clamp(--stage, 0, Levels.Length - 1);
			// UpdateLevel();

			UpdateLevel(-1);
		}
	}

	void UpdateLevel(int increment) {
		if (stage + increment >= Levels.Length) {
			MaxLevel = true;
			Debug.Log("EXCEEDED MAX LEVEL");
			return;
		}

		SetLevelTo(Mathf.Clamp(stage + increment, 0, Levels.Length - 1));
	}

	void SetLevelTo(int level) {
		if (_HighestStage <= level) {
			_HighestStage = level;
			stage = level;
		}
		else if (level < _HighestStage && level > stage) {
			stage = _HighestStage;
		}
		else {
			stage = level;
		}

		Levels[stage].Reset();
		UpdateAV();
	}

	void UpdateAV ()
	{
		Debug.Log ("updating AV, stage is now " + stage);
		//TODO: @ckromero move this to ExpManager. 
		//LevelsManager should be concerned with level accumulation.

		if (stage < Levels.Length && stage != 0) {
			// audioManager.ChangePad (newPad);

			// clapManager.UpdateClappers(Levels[stage].ClapLevel);
			expManager.LevelChanged(stage);

//			animatorManager.changeAnimationState (newState);
//			lightsManager.changeLightState(newState);
		}
	}

	public void StartLevels ()
	{ 
		listenToGestures = true;
	}
	public void StopLevels ()
	{
        Debug.Log("StopLevels called");
		listenToGestures = false;
		Performing = false;
		stage = 0;
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
	public ClapTrigger ClapLevel;
	public int Advancement = 1;

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

	public void Decrement() {
		_LevelStatus--;
	}

	public void Incrememnt() {
		_LevelStatus++;
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
