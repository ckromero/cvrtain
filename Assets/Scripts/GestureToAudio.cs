using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureToAudio : MonoBehaviour
{

	public GestureManager gestureManager;
	public AudioManager audiomanager;
	public LevelsManager levelsManager;
	public ExpManager expManager;

	public float TimeBetweenGestures;

	private CompletedGestureStruct completedGEstureStruct;
	private Gesture[] gestures;
	private Dictionary <string, string> gestureToAudioList;
	private float lastGestureTime = 0.0f;
	private bool IsFirstGesture=true;
	private CompletedGestureStruct checkLastGesture;
	void Awake ()
	{
		SetUpGestureToAudioList ();
	}
	// Use this for initialization
	void Start ()
	{
		gestures = gestureManager.Gestures;
		CheckGestureToAudio ();
	}

	void CheckGestureToAudio ()
	{ 
		//check gestue list and see if we have corresponding audio.
		//ensure values in dictionary list exist as game objects. 
//		Debug.LogWarning ("Warning if gesture doesn't have corresponding audio");

	}

	public void Reset() { 
		IsFirstGesture=true;
		lastGestureTime = Time.time;
	}


	void Update ()
	{
		if (levelsManager.Performing) {
			
			//if a new gesture has been detected
			checkLastGesture = gestureManager.LastGesture;
			if (checkLastGesture.Time != lastGestureTime) { 
				HandleGesture (checkLastGesture);
				lastGestureTime = checkLastGesture.Time;
			}
		}

	}

	void HandleGesture (CompletedGestureStruct _completedGestureStruct)
	{
		string lastGesture = _completedGestureStruct.Name;

		if (IsFirstGesture && lastGesture!="Weird dance") { 
			expManager.SendCue("StopIntro");
			IsFirstGesture = false;
		}
		//consider ignoring if last trigger was too soon
		//TODO: @ckromero add complex cues (hands out cuts off audience on level 3)
		//TODO: @ckromero think about using alt sound cues too

		if (gestureToAudioList.ContainsKey (lastGesture)) {
			string soundToPlay = gestureToAudioList [lastGesture];
			Debug.Log ("ready to trigger " + soundToPlay );
//			audiomanager.TriggerSound ("missing sound");
		} else {
			Debug.LogWarning ("no sound for " + lastGesture);
		}
	}

	private void SetUpGestureToAudioList ()
	{
		gestureToAudioList = new Dictionary<string, string> ();

		gestureToAudioList.Add ("Arms out, basking", "ArmsToSide_EDIT");
		gestureToAudioList.Add ("gesture facing away", "Aww booo booo_EDITED");
		gestureToAudioList.Add ("Blow Kisses", "BlowKisses_EDIT");
		gestureToAudioList.Add ("Bow", "Bow_EDIT");
		gestureToAudioList.Add ("Deep bow", "DeepBow_EDIT");
		gestureToAudioList.Add ("Hand Slice", "HandSlice_EDIT");
		gestureToAudioList.Add ("Hands Together", "HandsTogetherShaking_EDIT");
		gestureToAudioList.Add ("Hands up bow", "HandsUpBow_EDIT");
		//*********** TODO: @ckromero: Lets break this up into left and right!
		gestureToAudioList.Add ("Hand Sweep", "HandSweepUpL_EDIT");
		gestureToAudioList.Add ("Hand to heart bow", "HandToHeart_EDIT");
		gestureToAudioList.Add ("no moving", "NotMoving_EDIT");
		gestureToAudioList.Add ("One arm bow", "OneArmUp_EDIT");
		gestureToAudioList.Add ("One Hand High, One Hand Low", "OneHandHighOneLow_EDIT");
		gestureToAudioList.Add ("Pump it up", "PumpItUp_EDIT");
		gestureToAudioList.Add ("Wai", "TwoHandsTogether_EDIT");
		gestureToAudioList.Add ("Wave", "Wave_EDIT");	
		gestureToAudioList.Add ("Weird dance", "WierdDance_EDIT");
		gestureToAudioList.Add ("Weird random", "WierdRandom_EDIT");

		//gestureToAudioList.Add ("Testing bow", "ssob-2.wav");
		//gestureToAudioList.Add ("Curtsy", "cheer yip swell 6.wav");

//		gestureToAudioList.Add ("Hand sweep upper right", "Whistle clean highpass.wav");

//		Bow		ssob-2.wav
//		Deep bow		SSO5.wav
//		Hands up bow	needs to be found/made	Swell with cheerrs
//		gesture facing away		Aww booo booo.wav
//		Pump it up		cheer yip swell_5.wav
//		One arm up bow		large applause 3.wav
//		Arms out to side		Cheer yip swell 2.wav
//		Hand Sweep upper left 		bravo.wav
//		Hand sweep upper right		Whistle clean highpass.wav
//		Right hand to heart, bow		Cheer yip swell 3.wav
//		not moving		Medium audience clapping, good natural taper.wav
//		Curtsie		cheer yip swell 6
//		Weird dance		Laughter3.wav
//		Weird random		Laughter 2.wav
//		Wave		
//		Hand slice, everybody stop clapping	needs to be created	master room tone
//		Blow kisses?		foot stomping (fireworks).wav
//		Hands together & shaking		Cheer yip swell_4.wav
//		One hand high, one low	needs to be found/made	medium swell
//		two hands together bow	needs to be found/made	large swell




	}



}
