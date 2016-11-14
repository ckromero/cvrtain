using UnityEngine;
using System.Collections;

public class ExpManager : MonoBehaviour
{
	private enum ExpStates {Idle,IntroScreen,InstructionScreen,CurtainOpen,Levels,CurtainClose,Outro,Titles}; 
	public bool IsIdle,IsIntroScreen,IsInstructionScreen,IsCurtainOpen,IsLevels,IsCurtainClose,IsOutro,IsTitles;
	private bool[] states;
	public string state = "";
	private ExpStates expState;

	void Update() {
//		states = new bool[]{ IsIdle, IsIntroScreen, IsInstructionScreen, IsCurtainOpen, IsLevels, IsCurtainClose, IsOutro, IsTitles };
		if (IsIdle) { 
			expState = ExpStates.Idle;
			IsIdle = false;
		}
		if (IsIntroScreen) { 
			expState = ExpStates.IntroScreen;
			IsIntroScreen = false;
		}
		if (IsInstructionScreen) { 
			expState = ExpStates.InstructionScreen;
			IsInstructionScreen = false;
		}
		if (IsCurtainOpen) { 
			expState = ExpStates.CurtainOpen;
			IsCurtainOpen = false;
		}
		if (IsLevels) { 
			expState = ExpStates.Levels;
			IsLevels = false;
		}
		if (IsCurtainClose) { 
			expState = ExpStates.CurtainClose;
			IsCurtainClose = false;
		}
		if (IsOutro) { 
			expState = ExpStates.Outro;
			IsOutro = false;
		}
		if (IsTitles) { 
			expState = ExpStates.Titles;
			IsTitles = false;
		}
		state = expState.ToString ();

	}




}

