using System.Collections;
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
		postShowPleased,
		postShowNeutral,
		postShowDisappointed,
		quiet}
	;

	public AudioSource[] additionalMurmurSounds;
	public AudioSource[] additionalPoliteSounds;
	public AudioSource[] additionalMediumSounds;
	public AudioSource[] additionalLargeSounds;
	public AudioSource[] additionalHugeSounds;
	public AudioSource[] additionalPostShowPleasedSounds;
	public AudioSource[] additionalPostShowNeutralSounds;
	public AudioSource[] additionalPostShowDisappointedSounds;

	private AudienceState audState;
	private  string currentAudioMixerSnapshot;

	void Awake ()
	{
		ChangePad ("murmur");
	}

	void Start ()
	{ 
		GameObject goc1 = GameObject.Find ("Coughs1");
		GameObject goc2 = GameObject.Find ("Coughs2");

		AudioSource c1 = goc1.GetComponent<AudioSource> ();
		AudioSource c2 = goc2.GetComponent<AudioSource> ();

		additionalMurmurSounds = new AudioSource[]{ c1, c2 };

		StartCoroutine (AdditionalSoundsBasedOnPads ());
	}

	void Update ()
	{



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
			case AudienceState.postShowPleased:
				PickSoundToPlay (additionalPostShowPleasedSounds);
				break;
			case AudienceState.postShowNeutral:
				PickSoundToPlay (additionalPostShowNeutralSounds);
				break;
			case AudienceState.postShowDisappointed:
				PickSoundToPlay (additionalPostShowDisappointedSounds);
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


	public void ChangePad (string pad)
	{ 
		//Debug.Log ("ChangePad Received: " + pad);
		
		switch (pad) {
		case "murmurs":
			audState = AudienceState.murmur;	
			TransitionAudio ("murmur", timeForTransition);
			break;
		case "polite":
			audState = AudienceState.polite;	
			TransitionAudio ("polite", timeForTransition);
			break;
		case "medium":
			audState = AudienceState.medium;	
			TransitionAudio ("medium", timeForTransition);
			break;
		case "large":
			audState = AudienceState.large;	
			TransitionAudio ("large", timeForTransition);
			break;
		case "huge":
			audState = AudienceState.huge;	
			TransitionAudio ("huge", timeForTransition);
			break;
		case "postShowNeutral":
			audState = AudienceState.postShowPleased;	
			TransitionAudio ("postShowPleased", timeForTransition);
			break;
		case "postShowPleased":
			audState = AudienceState.postShowNeutral;	
			TransitionAudio ("postShowNeutral", timeForTransition);
			break;
		case "postShowDisappointed":
			audState = AudienceState.postShowDisappointed;	
			TransitionAudio ("postShowDisappointed", timeForTransition);
			break;
		case "allQuiet":
			audState = AudienceState.postShowDisappointed;	
			TransitionAudio ("quiet", timeForTransition);
			break;

		}
	}


	public void TriggerSound (string soundName)
	{ 

		Debug.Log ("Trigger Sound received: " + soundName);
		switch (soundName) { 
		case "AhhClap":
			TriggerAudio ("AhhClap");
			break;
		case "MildLaugh":
			TriggerAudio ("MildLaugh");
			break;
		case "Coughs1":
			TriggerAudio ("Coughs1");
			break;
		case "Coughs2":
			TriggerAudio ("Coughs2");
			break;
		case "Tympani":
			TriggerAudio ("12082016123444_DN-700R");
			break;

		case "SWITCH_1":
			TriggerAudio ("SWITCH_1");
			break;
		}
	}


	private void TriggerAudio (string audioName)
	{ 
		// TODO: ignore playing a sound file if said file is already playing	
		GameObject goAudio = GameObject.Find (audioName);
		AudioSource audioToPlay = goAudio.GetComponent<AudioSource> ();
		Debug.Log ("audiotoplay: " + audioToPlay);
		audioToPlay.Play ();



//		Vector3 playbackPosition = goAudio.GetComponentInParent<Transform> ().position;
//		Vector3 pos = Vector3.zero;
//		if (location != "") { 
//			GameObject goLocation = GameObject.Find (location);
//			pos = goLocation.transform.position;
//		}



//		//TODO: Optimize
//		foreach (AudioSource a in audios) { 
//			if (a.clip.name == audioName) {
//				audioToPlay = a;
//				break;
//			}
//		}
//		
//		if (audioToPlay != null) {
//			audioToPlay.Play ();
//		}
	}



	private void TransitionAudio (string amsToName, float timeForTransition, float weight = 1.0f)
	{ 
		//Debug.Log ("TransitionAudio received " + amsToName);
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
