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
		if (expManager.IsInHandSlice) {
			expManager.StopHandSliceCo ();

		}
        Debug.Log("lastgesture is: " + lastGesture);

        if (gestureToAudioList.ContainsKey(lastGesture)) {

            if (lastGesture == "Hand Slice") {
                expManager.HandleHandSlice();
            } else if (lastGesture == "Laughable") {
                //laughable
                audiomanager.PlayWeirdSound();
            } else if (lastGesture == "Backwards") {
                Debug.Log("Backwards gesture");
                audiomanager.PlayReverseSounds();

            } else {
            string soundToPlay = gestureToAudioList[lastGesture];
            Debug.Log("ready to trigger " + soundToPlay);
            audiomanager.TriggerSound(soundToPlay, "gesture");
        }

		} else {
			Debug.LogWarning ("no sound for " + lastGesture);
		}
	}

	private void SetUpGestureToAudioList ()
	{
		gestureToAudioList = new Dictionary<string, string> ();

		gestureToAudioList.Add ("Arms out, basking", "1229_ArmsToSide");
		gestureToAudioList.Add ("Blow a Kiss", "1230_BlowKisses");
		gestureToAudioList.Add ("Bow", "1229_Bow-Norm");
		gestureToAudioList.Add ("Deep bow", "1229_DeepBow");
		gestureToAudioList.Add ("Hand Slice", "1229_HandUpLeft");
		gestureToAudioList.Add ("Hands Together", "1229_HandsTogetherShake");
		gestureToAudioList.Add ("Hands up bow", "1229_HandsUp");
		gestureToAudioList.Add ("One arm sweep left", "1229_HandUpLeft");
		gestureToAudioList.Add ("One arm sweep right", "1230_HandUpRight");
		gestureToAudioList.Add ("Hand to heart bow", "1229_RHandToHeart");
//		gestureToAudioList.Add ("no moving", "1226_NotMoving");
		gestureToAudioList.Add ("One arm bow", "1229_OneArmUp");
		gestureToAudioList.Add ("One Hand High, One Hand Low", "1229_OneHighOneLow");
		gestureToAudioList.Add ("Pump it up", "1229_PumpItUp");
		gestureToAudioList.Add ("Wai", "1229_TwoHandsTogether");
		gestureToAudioList.Add ("Wave", "1229_Wave");	
		gestureToAudioList.Add ("Weird dance", "1230_AVAILABLE");	
		gestureToAudioList.Add ("First Backwards", "1229_FirstBackwards");
		gestureToAudioList.Add ("Laughable", "1226_WierdDance");
		gestureToAudioList.Add ("Two Hands Up", "1230_TwoArmsUp");
		gestureToAudioList.Add ("Backwards", "1230_TwoArmsUp");

//		gestureToAudioList.Add ("gesture facing away", "1226_FacingAway");

	}

}

//1229_ArmsToSide.wav
//1229_BlowKisses.wav
//1229_Bow-Norm.wav
//1229_Curtsie-Norm.wav
//1229_DeepBow.wav
//1229_FacingAway.wav
//1229_HandUpLeft.wav
//1229_HandUpRight.wav
//1229_HandsTogetherShake.wav
//1229_HandsUp.wav
//1229_OneArmUp.wav
//1229_OneHighOneLow.wav
//1229_PumpItUp.wav
//1229_RHandToHeart.wav
//1229_TwoHandsTogether.wav
//1229_Wave.wav