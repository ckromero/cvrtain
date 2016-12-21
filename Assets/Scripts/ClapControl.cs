using UnityEngine;
using System.Collections;

public class ClapControl : MonoBehaviour
{

	public bool triggerGoToHigh = false;
	public bool triggerRaisedFist = false;
	public bool triggerHighClapping = false;
	public bool triggerMildClapping = false;
	public float lowerSpeedRange = 0.8f;
	public float upperSpeedRange = 1.2f;
	private Animator animator;


	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (triggerGoToHigh) { 
			TriggerGoToHigh ();
//			Debug.Log ("Updating Clap Control: TriggerGoToHigh");
			triggerGoToHigh = false;
		}
		if (triggerRaisedFist) {
			TriggerRaisedFist ();
			Debug.Log ("Updating Clap Control: TriggerRaisedFist");
			triggerRaisedFist = false;
		}

		if (triggerHighClapping) {
			TriggerHighClapping ();
//			Debug.Log ("Updating Clap Control: TriggerHighClapping");
			triggerHighClapping = false;
		}			
		if (triggerMildClapping) { 
			TriggerMildClapping ();
//			Debug.Log ("Updating Clap Control: Trigger Mild Clapping");
			triggerMildClapping = false;
		}
	}

	public void WhichTrigger (string triggerName)
	{
		Debug.Log ("WhichTrigger called with: " + triggerName);
		switch (triggerName) { 
		case "triggerRaisedFist":
			triggerRaisedFist = true;
			break;
		case "triggerHighClapping":
			triggerHighClapping = true;
			break;
		case "triggerMildClapping":
			triggerMildClapping = true;
			break;
		case "":
			Debug.Log ("WhichTrigger called with empty targetTrigger");
			break;
		default:
			Debug.Log ("WhicTrigger called with a targetTrigger it didn't understand");	
			break;
		}
	}

	void TriggerGoToHigh ()
	{
//		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("mild clapping animation")) {
		animator.SetTrigger ("T_mildToHighClap");
		AdjustSpeed ();	

//		}
	}

	void TriggerRaisedFist ()
	{
//		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("high clapper")) {
		animator.SetTrigger ("T_highClapperToRaisedFist");
		AdjustSpeed ();	
//		}
	}

	void TriggerHighClapping ()
	{ 

//		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("fist pump animation")) {
//		animator.SetTrigger ("T_raisedFistToHighClapper");
		animator.SetTrigger ("T_highClapper");
		AdjustSpeed ();	
//		}
	}

	void TriggerMildClapping ()
	{ 

//		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("fist pump animation")) {
		animator.SetTrigger ("T_GoToMild");
		AdjustSpeed ();	
//		}
	}


	void AdjustSpeed ()
	{ 

		animator.speed = Random.Range (lowerSpeedRange, upperSpeedRange);

	}
}
