﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureToAudio : MonoBehaviour
{

	public GestureManager gestureManager;
	public AudioManager audiomanager;
	public float TimeBetweenGestures;

	private CompletedGestureStruct completedGEstureStruct;
	private Gesture[] gestures;
	private Dictionary <string, string> gestureToAudioList;
	private float lastGestureTime = 0.0f;

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
		Debug.LogWarning ("Warning if gesture doesn't have corresponding audio");

	}
	// Update is called once per frame
	void Update ()
	{
		//if a new gesture has been detected
		CompletedGestureStruct checkLastGesture = gestureManager.LastGesture;
		if (checkLastGesture.Time != lastGestureTime) { 
			HandleGesture (checkLastGesture);
			lastGestureTime = checkLastGesture.Time;
		}

	}

	void HandleGesture (CompletedGestureStruct _completedGestureStruct)
	{
		//consider ignoring if last trigger was too soon
		//TODO: @ckromero add complex cues (hands out cuts off audience on level 3)
		//TODO: @ckromero think about using alt sound cues too
		string lastGesture = _completedGestureStruct.Name;
		if (gestureToAudioList.ContainsKey (lastGesture)) {
			string soundToPlay = gestureToAudioList [lastGesture];
			Debug.Log ("ready to triger " + soundToPlay + " but triggering missing sound for now");
			audiomanager.TriggerSound ("missing sound");
		} else {
			Debug.LogWarning ("no sound for " + lastGesture);
		}
	}

	private void SetUpGestureToAudioList ()
	{
		gestureToAudioList = new Dictionary<string, string> ();

		gestureToAudioList.Add ("Hands up bow", "missing sound.wav");
		gestureToAudioList.Add ("One Hand High, One Hand Low", "missing sound.wav");
		gestureToAudioList.Add ("Pump it up", "cheer yip swell_5.wav");
		gestureToAudioList.Add ("Deep bow", "SSO5.wav");
		gestureToAudioList.Add ("Bow", "ssob-2.wav");
		gestureToAudioList.Add ("Testing bow", "ssob-2.wav");
		gestureToAudioList.Add ("Wave", "missing sound.wav");	
		//agnostic, right or left
		gestureToAudioList.Add ("Hand Sweep", "bravo.wav");
		gestureToAudioList.Add ("One arm bow", "large applause 3.wav");
		gestureToAudioList.Add ("Arms out, basking", "cheer yip swell 2.wav");
		gestureToAudioList.Add ("Curtsy", "cheer yip swell 6.wav");
		gestureToAudioList.Add ("Hand to heart bow", "Cheer yip swell 3.wav");
		gestureToAudioList.Add ("Wai", "missing sound.wav");
		gestureToAudioList.Add ("Hands Together", "Cheer yip swell_4.wav");




		gestureToAudioList.Add ("gesture facing away", "Aww boo boo.wav");

//		gestureToAudioList.Add ("Hand sweep upper right", "Whistle clean highpass.wav");

		gestureToAudioList.Add ("no moving", "Medium audience clapping, good natural taper.wav");

		gestureToAudioList.Add ("Weird dance", "Laughter3.wav");
		gestureToAudioList.Add ("Weird random", "Laughter 2.wav");

		gestureToAudioList.Add ("Hand Slice", "missing sound.wav");
		gestureToAudioList.Add ("Blow Kisses", "foot stomping (fireworks).wav");





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