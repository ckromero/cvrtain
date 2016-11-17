using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {

	public Canvas[] canvasSet;

	public ExpManager expManager;

	void Update() { 
		//Idle,IntroScreen,InstructionScreen,CurtainOpen,Levels,CurtainClose,Outro,Titles

		if (expManager.expState == ExpManager.ExpStates.Idle) 
			UpdateHUDState ("Waiting");
		
		if (expManager.expState == ExpManager.ExpStates.Intro) 
			UpdateHUDState ("Intro");			

		if (expManager.expState == ExpManager.ExpStates.Instructions) 
			UpdateHUDState ("Instructions");

		if (expManager.expState == ExpManager.ExpStates.CurtainOpen || expManager.expState == ExpManager.ExpStates.Levels || expManager.expState == ExpManager.ExpStates.CurtainClose ) 
			UpdateHUDState ("Levels");	

		if (expManager.expState == ExpManager.ExpStates.Outro) 
			UpdateHUDState ("Outro");
		if (expManager.expState == ExpManager.ExpStates.Credits) 
			UpdateHUDState ("Credits");
	}
	//Waiting, Intro, Instructions, Levels, Outro, Credits
	void UpdateHUDState(string canvasName) { 
		foreach (Canvas canvas in canvasSet) { 
			if (canvas.name == canvasName) { 
				canvas.gameObject.SetActive (true);
			} else {
				canvas.gameObject.SetActive (false);
			}
		}
	}
}
