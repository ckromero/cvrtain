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


	public void TriggerSound (string soundName)
	{ 
//		Debug.Log ("Trigger Sound received: " + soundName);

		TriggerAudio (soundName);
	}

	public void StopSound (string soundName)
	{ 
		StopAudio (soundName);
	}

	private void TriggerAudio (string audioName)
	{ 
		// TODO: ignore playing a sound file if said file is already playing	
		GameObject goAudio = GameObject.Find (audioName);
		AudioSource audioToPlay = goAudio.GetComponent<AudioSource> ();
		if (audioToPlay.isPlaying) { 
			Debug.Log (audioToPlay.ToString () + " is already playing!");
		} else {
			Debug.Log ("audioToPlay: " + audioToPlay);
			audioToPlay.Play ();
		}
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
