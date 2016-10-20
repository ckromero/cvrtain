using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public GameObject audioObject;
	public AudioMixer audioMixer;
	public float timeForTransition;



	private  string currentAudioMixerSnapshot;
	private AudioSource[] audios;

	void Awake ()
	{
		TransitionAudio ("just room tone", timeForTransition);
	}

	void Start ()
	{ 
		audios = audioObject.GetComponents<AudioSource> ();

	}

	public void ChangeClappingType (string type)
	{
		Debug.Log ("ChangeClappingType received " + type);

		switch (type) {
		case "small":
			TransitionAudio ("small applause", timeForTransition);
			break;

		case "medium":
			TransitionAudio ("medium applause", timeForTransition);
			break;

		case "large":
			TransitionAudio ("large applause", timeForTransition);
			break;

		case "room":
			TransitionAudio ("just room tone", timeForTransition);
			break;


		}



	}


	public void TriggerSound (string soundName)
	{ 

		Debug.Log ("Trigger Sound received: " + soundName);
		switch (soundName) { 
		case "bravo":
			TriggerAudio ("bravo");
			break;
		case "Distant Cheers":
			TriggerAudio ("Distant Cheers");
			break;
		case "Rhythmic":
			TriggerAudio ("Rhythmic 3");
			break;
		}
	}


	private void TriggerAudio (string audioName)
	{ 
		AudioSource audioToPlay = new AudioSource ();

		//TODO: Optimize
		foreach (AudioSource a in audios) { 
			if (a.clip.name == audioName) {
				audioToPlay = a;
				break;
			}
		}
		
		if (audioToPlay != null) {
			audioToPlay.Play ();
		}
	}



	private void TransitionAudio (string amsToName, float timeForTransition, float weight = 1.0f)
	{ 
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
