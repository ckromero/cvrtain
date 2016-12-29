using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public GameObject audioObject;
	public AudioMixer audioMixer;
	public float timeForTransition;
	public float playNewSoundPercentage = 0.9f;
	public float playNewSoundWait = 1.5f;

	public enum AudienceState
	{
		murmur,
		showOpen,
		polite,
		medium,
		large,
		huge,
		showClose,
		postShow,
		quiet}
	;

	public AudioSource[] additionalMurmurSounds;
	public AudioSource[] additionalPoliteSounds;
	public AudioSource[] additionalMediumSounds;
	public AudioSource[] additionalLargeSounds;
	public AudioSource[] additionalHugeSounds;
	public AudioSource[] additionalPostShowSounds;

	private AudienceState audState;
	private string soundToFade="";
	private float soundToFadeAudioLevel=1.0f;

	private string[] wierdAudioList = new string[] { "WierdRandom1",
		"WierdRandom2",
		"WierdRandom3",
		"WierdRandom4",
		"WierdRandom5",
		"WierdRandom6",
		"WierdRandom7",
		"WierdRandom8",
		"WierdRandom9",
		"WierdRandom10",
		"WierdRandom11",
		"WierdRandom12",
		"WierdRandom13"
	};

	private string lastGestureAltTriggered = "";
	private string[] gestureAlts = new string[]{ "BowAlt_01", 
		"BowAlt_02", 
		"BowAlt_03", 
		"BowAlt_04", 
		"BowAlt_05",
		"BowAlt_06", 
		"BowAlt_07", 
		"BowAlt_08", 
		"BowAlt_09", 
		"BowAlt_10",
		"BowAlt_11",
		"BowAlt_12",
		"BowAlt_13",
		"BowAlt_14",
		"BowAlt_15",
		"BowAlt_16",
		"BowAlt_17",
		"BowAlt_18",
		"BowAlt_19",
		"BowAlt_20",
		"BowAlt_21"};

	private  string currentAudioMixerSnapshot;

	void Awake ()
	{
		ChangePad ("murmur");
	}

	void Start ()
	{ 
//		GameObject goc1 = GameObject.Find ("Coughs1");
//		GameObject goc2 = GameObject.Find ("Coughs2");

//		AudioSource c1 = goc1.GetComponent<AudioSource> ();
//		AudioSource c2 = goc2.GetComponent<AudioSource> ();

//		additionalMurmurSounds = new AudioSource[]{ c1, c2 };
//		additionalMurmurSounds = new AudioSource[]{ c1, c2 };

//		StartCoroutine (AdditionalSoundsBasedOnPads ());
	}

	void Update ()
	{
		FadeOut ();

	}

	IEnumerator AdditionalSoundsBasedOnPads ()
	{
		while (true) { 
			switch (audState) { 
			case AudienceState.murmur:
				PickSoundToPlay (additionalMurmurSounds);
				break;
			case AudienceState.polite:
				PickSoundToPlay (additionalPoliteSounds);
				break;
			case AudienceState.medium:
				PickSoundToPlay (additionalMediumSounds);
				break;
			case AudienceState.large:
				PickSoundToPlay (additionalLargeSounds);
				break;
			case AudienceState.huge:
				PickSoundToPlay (additionalHugeSounds);
				break;
			case AudienceState.postShow:
				PickSoundToPlay (additionalPostShowSounds);
				break;
			}

			yield return new WaitForSeconds (playNewSoundWait);	
		}
	}

	void PickSoundToPlay (AudioSource[] audioArray)
	{
		if (audioArray.Length > 0) { 
			float soundsIndex = Random.Range (0, audioArray.Length); 
			AudioSource audioToPlay = audioArray [(int)soundsIndex];

			if (Random.value > playNewSoundPercentage) {
				TriggerAudio (audioToPlay.transform.name);
//				TriggerSound (audioToPlay.transform.name);
				Debug.Log ("AdditionalSoundsBasedOnPads triggered " + audioToPlay.transform.name);

			}	
		}
	}

	public void ChangePadSimple (string newPad)
	{
		ChangePad (newPad);
	}

	public void ChangePad (string pad, float transitionTime = 0)
	{ 
		Debug.Log ("ChangePad Received: " + pad);
		if (transitionTime == 0) {
			transitionTime = timeForTransition;
		}
		switch (pad) {
		case "quiet":
			audState = AudienceState.murmur;	
			TransitionAudio ("quiet", transitionTime);
			break;
		case "murmur":
			audState = AudienceState.murmur;	
			TransitionAudio ("murmur", transitionTime);
			break;
		case "polite":
			audState = AudienceState.polite;	
			TransitionAudio ("polite", transitionTime);
			TriggerSound ("IntoSmall_EDIT");
			break;
		case "medium":
			audState = AudienceState.medium;	
			TransitionAudio ("medium", transitionTime);
			TriggerSound ("Into Medium_EDIT");
			break;
		case "large":
			audState = AudienceState.large;	
			TransitionAudio ("large", transitionTime);
			TriggerSound ("Into Large_EDIT");
			break;
		case "huge":
			audState = AudienceState.huge;	
			TransitionAudio ("huge", transitionTime);
			break;
		case "postShowNeutral":
			audState = AudienceState.postShow;	
			TransitionAudio ("postShowPleased", transitionTime);
			break;
		}
	}


	public void TriggerSound (string soundName,string gesturePassthrough="")
	{ 
//		Debug.Log ("Trigger Sound received: " + soundName);

		TriggerAudio (soundName,gesturePassthrough);
	}

	public void StopSound (string soundName)
	{ 
		StopAudio (soundName);
	}

	private void TriggerAudio (string audioName, string triggeredGesture = "")
	{ 
		// TODO: ignore playing a sound file if said file is already playing	
		GameObject goAudio = GameObject.Find (audioName);
		AudioSource audioToPlay = goAudio.GetComponent<AudioSource> ();
		if (audioToPlay.isPlaying) { 
			if (triggeredGesture == "gesture") {
				string pickedAlt = PickAlt ();
				GameObject pickedAltAudio = GameObject.Find (pickedAlt);
				AudioSource pickedAltAudioToPlay = pickedAltAudio.GetComponent<AudioSource> ();
				pickedAltAudioToPlay.Play ();

			} else {
			
				string wierdSound = PickWierdAudio ();
				Debug.Log (audioToPlay.ToString () + " is already playing, so playing " + wierdSound + " instead.");
				GameObject goWierdAudio = GameObject.Find (wierdSound);
				AudioSource wierdAudioToPlay = goWierdAudio.GetComponent<AudioSource> ();
				wierdAudioToPlay.Play ();
			}
		} else {
			Debug.Log ("audioToPlay: " + audioToPlay);
			audioToPlay.Play ();
		}
	}


	public void SetSoundToFade(string _soundToFade){
		Debug.Log ("SetSoundToFade received: " + _soundToFade);
		soundToFade = _soundToFade;
	}

	private void FadeOut(){
		if (soundToFade != "") {
			GameObject fadingAudioGO = GameObject.Find (soundToFade);
			AudioSource fadingAudioSource = fadingAudioGO.GetComponent<AudioSource> ();
			if (soundToFadeAudioLevel > 0.1) {
				soundToFadeAudioLevel -= 0.1f * Time.deltaTime;

				fadingAudioSource.volume = soundToFadeAudioLevel;
				Debug.Log (soundToFade + " now has audioLevel " + soundToFadeAudioLevel);
			} else {
				fadingAudioSource.Stop ();
				soundToFade = "";
			}
		}			
	}


	private string PickAlt ()
	{ 
		string pickedAlt = "";

		int length = gestureAlts.Length;
		int altIndex;

		if (lastGestureAltTriggered == "") {
			pickedAlt = gestureAlts [0];
		} else {
			int underscoreIdx = lastGestureAltTriggered.IndexOf ("_");
			string intSuffix = lastGestureAltTriggered.Substring (underscoreIdx + 1);
			int altNumber = int.Parse (intSuffix);


			if (altNumber == length) {
				pickedAlt = gestureAlts [0];
			} else {
				//not the last alt
				string pack = "";
				if (altNumber + 1 <= 9) {
					pack = "0";
				}
				//alts root with underscore, packing zero if needed, increment the altNumber.	
				altNumber++;
				pickedAlt = lastGestureAltTriggered.Substring (0, underscoreIdx + 1) + pack + altNumber.ToString ();

			}
		}

		Debug.Log ("PickedAlt: " + pickedAlt);
		lastGestureAltTriggered=pickedAlt;

		return pickedAlt;

	}

	private string PickWierdAudio ()
	{ 
		int length = wierdAudioList.Length;
		int whichWierd = Random.Range (0, length - 1);
		string wierdSoundToPlay = wierdAudioList [whichWierd];
		return wierdSoundToPlay;
	}


	private void StopAudio (string audioName)
	{ 
		GameObject goAudio = GameObject.Find (audioName);
		AudioSource audioToStop = goAudio.GetComponent<AudioSource> ();
		Debug.Log ("audioToStop: " + audioToStop);
		audioToStop.Stop ();

	}

	private void TransitionAudio (string amsToName, float timeForTransition, float weight = 1.0f)
	{ 
		Debug.Log ("TransitionAudio received " + amsToName);
		AudioMixerSnapshot ams = audioMixer.FindSnapshot (amsToName);
		AudioMixerSnapshot[] amsArray = new AudioMixerSnapshot[]{ ams };
		float[] weightArray = new float[]{ weight };

		audioMixer.TransitionToSnapshots (amsArray, weightArray, timeForTransition); 
		currentAudioMixerSnapshot = ams.name;
	}

	public string currentSnapshot ()
	{
		if (currentAudioMixerSnapshot != null) {
			return currentAudioMixerSnapshot;
		} else {
			return "";
		}
	}

}
