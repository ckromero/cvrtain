using UnityEngine;
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


	void Update() {
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

