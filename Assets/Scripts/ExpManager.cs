using UnityEngine;
using System.Collections;

public class ExpManager : MonoBehaviour
{
	public AudioManager audioManager;
	public LevelsManager levelsManager;
	public LightsController lightsController;
	public ClapperManager clapperManager;

	public GameObject leftCurtainController;
	public GameObject rightCurtainController;
	public GameObject afterCurtainOpen;

	public DocentPrompter docentPrompter;
	private bool IsListenForSpaceBar=true;

	public enum ExpStates
	{
		Idle,
		Intro,
		Instructions,
		CurtainOpen,
		Levels,
		CurtainClose,
		Outro,
		Credits}
	;

	public ExpStates expState;

    public float showTime;

    public string state = "";
	public bool IsIdle, IsIntroScreen, IsInstructionScreen, IsCurtainOpen, IsLevels, IsCurtainClose, IsOutro, IsCredits;
	public bool IsRestartAnimation = false;

	private TriggerListener triggerListener;

	private float lastTriggerTime;
	private float startTrackingTime;
	private bool[] states;
	private bool IsStartShowNotificationSent = false;
	private bool IsStartTimer = false;
	private bool IsCheckTimer = false;
	private bool IsCurtainNotificationSent = false;
	private bool IsCurtainCalling = false;
	private string padToStore;
	private IEnumerator coroutine;
	public bool IsInHandSlice=false;

	void Start ()
	{
		triggerListener = GetComponent<TriggerListener> ();
		//TODO: need an in game trigger to restart the animation
		IsRestartAnimation = true;
	}


	void Update ()
	{
		if (IsStartTimer) { 
			startTrackingTime = Time.time;
			IsStartTimer = false;
			IsCheckTimer = true;
		}
		//how do we handle this on reset?

		if (IsCheckTimer && (Time.time - startTrackingTime > showTime)) { 
			SendCue ("CurtainCalling");
			IsCheckTimer = false;
		}

		if (Input.GetKeyDown ("space")) {
			//TODO: add calibration check after reset 
			print ("space key was pressed");
			RestartCurtainAnimation ();
			IsListenForSpaceBar = false;
		}
		
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
			SendCue ("ShowsOver");
			IsOutro = false;
		}
		if (IsCredits) { 
			expState = ExpStates.Credits;
			audioManager.ChangePad ("murmurs");
			IsCredits = false;
		}
		state = expState.ToString ();

	}

	public void SendCue (string cueName)
	{
		Debug.Log ("ExpManager.SendCue received: " + cueName);
		switch (cueName) {
		case "CurtainOpened":
			CurtainOpened ();
			break;
		case "StopIntro":
			StopIntro ();
			break;
		case "FirstCough":
			audioManager.TriggerSound ("single cough");
			break;
		case "AnotherCough":
			audioManager.TriggerSound ("coupla coughs");
			break;
		case "Whistle":
			audioManager.TriggerSound ("WhistleVerb_EDIT");
			break;
		case "TakeABow":
			audioManager.TriggerSound ("TakeABow_EDIT");
			break;
		case "CurtainCalling":
			CurtainCalling ();
			break;
		case "CurtainClosed":
			CurtainClosed ();
			break;
		case "ShowsOver":
			ShowsOver ();
			break;
		case "TurnOffMains":
			lightsController.TurnOffMains ();
			break;
		case "HouseLightsOn":
			lightsController.HouseToHalf ();
			break;
		}

	}

	public void LevelChanged (int levelNum)
	{ 
		//
		Debug.Log ("Level Changed:" + levelNum);
		switch (levelNum) {
		case 1:
			audioManager.ChangePad ("polite");
			clapperManager.ChangeLevel (2);
			break;
		case 2:
			audioManager.ChangePad ("medium");
			clapperManager.ChangeLevel (3);
			break;
		case 3:
			audioManager.ChangePad ("large");
			clapperManager.ChangeLevel (4);
			break;
		case 4:
			audioManager.ChangePad ("huge");
			clapperManager.ChangeLevel (5);
			break;
		}

		clapperManager.ChangeLevel (levelNum);

	}

	private void StopIntro ()
	{ 
		afterCurtainOpen.SendMessage ("ResetAnimation");
//		audioManager.StopSound ("RoomToneCoughv2_EDIT");	
		//audioManager.SetSoundToFade ("RoomToneCoughv2_EDIT");
	}

	private void CurtainOpened ()
	{ 
//		Debug.Log ("CurtainOpened called.");
		if (!IsCurtainNotificationSent) { 
			Debug.Log ("Experience Manager: Curtain is open");
			audioManager.TriggerSound ("SWITCH_1");
			audioManager.TriggerSound ("RoomToneCoughv2_EDIT");
			audioManager.TriggerSound ("SideTone");
			lightsController.TurnOnMains ();

			IsCurtainNotificationSent = true;

			levelsManager.BeginPerforming ();

			afterCurtainOpen.SendMessage ("StartAnimation");

			IsStartTimer = true;
		}
	}

	public void HandleHandSlice (){
		padToStore = audioManager.currentPad;
		audioManager.ChangePad ("quiet",0.1f);
        audioManager.StopAllTriggers();

        audioManager.TriggerSound("RoomToneCoughv2_EDIT");
        
        IsInHandSlice = true;
		coroutine=HandSliceCo();

		StartCoroutine (coroutine);
	}
	public void StopHandSliceCo(){
		StopCoroutine (coroutine);
		audioManager.ChangePad (padToStore);
        audioManager.SetSoundToFade("RoomToneCoughv2_EDIT");
        IsInHandSlice = false;
	}

	IEnumerator HandSliceCo() {
		yield return new WaitForSeconds(6);
		audioManager.TriggerSound ("HandSlice_EDIT");	
		yield return new WaitForSeconds(2);
		audioManager.ChangePad (padToStore);
        audioManager.SetSoundToFade("RoomToneCoughv2_EDIT");
        IsInHandSlice = false;
	}

	private void CurtainCalling() {
		Debug.Log ("CurtainCalling");

		audioManager.TriggerSound("TerryCloth_EDIT");
		leftCurtainController.SendMessage ("StartAnimation");
		rightCurtainController.SendMessage ("StartAnimation");
		lightsController.HouseToHalf();
		IsCurtainCalling = true;
	}
	public void CheckCurtainCalling() { 
		Debug.Log ("In CheckCurtainCalling");
		if (IsCurtainCalling) { 
			leftCurtainController.SendMessage ("EnableIsRollingBack");
			rightCurtainController.SendMessage ("EnableIsRollingBack");
		}
	}
	private void CurtainClosed() { 
		ShowsOver ();
	}
	private void ShowsOver ()
	{
		Debug.Log ("Shows Over");
//		audioManager.TriggerSound ("Aww1");

		audioManager.ChangePad ("murmur",5.0f);
        levelsManager.StopPerforming();
        levelsManager.StopLevels();
        docentPrompter.ExperienceOver();
		ResetShow ();
	}



	private void ResetShow ()
	{ 
		Debug.Log ("resetting show");
//		audioManager.SetSoundToFade ("Terry Cloth");
		lightsController.TurnOffMains ();
		levelsManager.StopPerforming();
		IsCurtainNotificationSent = false;
		IsCheckTimer = false;
		afterCurtainOpen.SendMessage ("ResetAnimation");
		audioManager.StopSound ("SideTone");
		// docentPrompter.ExperienceReset();
		IsListenForSpaceBar = true;
	}

	public void RestartCurtainAnimation ()
	{
		Debug.Log ("RestartAnimation() called");
		leftCurtainController.SendMessage ("StartAnimation");
		rightCurtainController.SendMessage ("StartAnimation");
		audioManager.TriggerSound ("tympani");	
		audioManager.ChangePad ("quiet", 10.0f);
		docentPrompter.ExperienceReset();
	}

}

