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
	private List<string> weirdRandoms;

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
			audiomanager.TriggerSound (soundToPlay,"gesture");
		} else {
			Debug.LogWarning ("no sound for " + lastGesture);
		}
	}

	private void SetUpGestureToAudioList ()
	{
		gestureToAudioList = new Dictionary<string, string> ();

		gestureToAudioList.Add ("Arms out, basking", "1226_ArmsToSide");
		gestureToAudioList.Add ("gesture facing away", "1226_FacingAway");
		gestureToAudioList.Add ("Blow Kisses", "BlowKisses_EDIT");
		gestureToAudioList.Add ("Bow", "1226_Bow");
		gestureToAudioList.Add ("Deep bow", "1226_DeepBow");
		gestureToAudioList.Add ("Hand Slice", "1226_HandUpLeft");
		gestureToAudioList.Add ("Hands Together", "1226_HandsTogetherShake");
		gestureToAudioList.Add ("Hands up bow", "1226_HandsUp");
		gestureToAudioList.Add ("One arm sweep left", "1226_HandUpLeft");
		gestureToAudioList.Add ("One arm sweep right", "1226_HandUpRight");
		gestureToAudioList.Add ("Hand to heart bow", "1226_RHandToHeart");
//		gestureToAudioList.Add ("no moving", "1226_NotMoving");
		gestureToAudioList.Add ("One arm bow", "1226_OneArmUp");
		gestureToAudioList.Add ("One Hand High, One Hand Low", "1226_OneHighOneLow");
		gestureToAudioList.Add ("Pump it up", "1226_PumpItUp");
		gestureToAudioList.Add ("Wai", "1226_TwoHandsTogether");
		gestureToAudioList.Add ("Wave", "1226_Wave");	
		gestureToAudioList.Add ("Weird dance", "1226_WierdDance");
	}

}
