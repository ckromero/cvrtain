﻿using UnityEngine;
using System.Collections;

public class ExpManager : MonoBehaviour
{
	public enum ExpStates {Idle,Intro,Instructions,CurtainOpen,Levels,CurtainClose,Outro,Credits}; 
	public bool IsIdle,IsIntroScreen,IsInstructionScreen,IsCurtainOpen,IsLevels,IsCurtainClose,IsOutro,IsCredits;
	private bool[] states;
	public string state = "";
	public ExpStates expState;

	public AudioManager audioManager;
	public LevelsManager levelsManager;
	private TriggerListener triggerListener = new TriggerListener();
	private float lastTriggerTime;

	void Start () { 
	}


	void Update() {
		if (triggerListener.LastDoublePress != Mathf.Infinity && triggerListener.LastDoublePress > lastTriggerTime) {
			lastTriggerTime = triggerListener.LastDoublePress;
			//TODO: AWFUL
			if (expState == ExpStates.Idle)			
				IsIntroScreen = true;
			if (expState == ExpStates.Intro)			
				IsInstructionScreen = true;
			if (expState == ExpStates.Instructions)			
				IsCurtainOpen = true;
			if (expState == ExpStates.CurtainOpen)			
				IsLevels = true;
			if (expState == ExpStates.Levels)			
				IsCurtainClose = true;
			if (expState == ExpStates.CurtainClose)			
				IsOutro = true;
			if (expState == ExpStates.Outro)			
				IsCredits = true;
			if (expState == ExpStates.Credits)			
				IsIdle = true;

		}

//		states = new bool[]{ IsIdle, IsIntroScreen, IsInstructionScreen, IsCurtainOpen, IsLevels, IsCurtainClose, IsOutro, IsTitles };
		if (IsIdle) { 
			expState = ExpStates.Idle;
			audioManager.ChangePad ("quiet");
			IsIdle = false;
		}
		if (IsIntroScreen) { 
			expState = ExpStates.Intro;
			audioManager.ChangePad ("murmurs");
			IsIntroScreen = false;
		}
		if (IsInstructionScreen) { 
			expState = ExpStates.Instructions;
			audioManager.ChangePad ("murmurs");
			IsInstructionScreen = false;
		}
		if (IsCurtainOpen) { 
			expState = ExpStates.CurtainOpen;
			audioManager.ChangePad ("quiet");
			IsCurtainOpen = false;
		}
		if (IsLevels) { 
			expState = ExpStates.Levels;
			levelsManager.StartLevels ();
			IsLevels = false;
		}
		if (IsCurtainClose) { 
			expState = ExpStates.CurtainClose;
			IsCurtainClose = false;
		}
		if (IsOutro) { 
			expState = ExpStates.Outro;
			audioManager.ChangePad ("murmurs");
			IsOutro = false;
		}
		if (IsCredits) { 
			expState = ExpStates.Credits;
			audioManager.ChangePad ("murmurs");
			IsCredits = false;
		}
		state = expState.ToString ();

	}




}

