using UnityEngine;
using System.Collections;

public class ClapControl : MonoBehaviour
{

	public bool triggerGoToHigh = false;
	public bool triggerRaisedFist = false;
	public bool triggerHighClapping = false;

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
			triggerGoToHigh = false;
		}
		if (triggerRaisedFist) {
			TriggerRaisedFist ();
			triggerRaisedFist = false;
		}

		if (triggerHighClapping) {
			TriggerHighClapping ();
			triggerHighClapping = false;
		}			
	}

	void TriggerGoToHigh ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("mild clapping animation")) {
			animator.SetTrigger("T_mildToHighClap");
		}
	}

	void TriggerRaisedFist ()
	{
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("high clapper")) {
			animator.SetTrigger ("T_highClapperToRaisedFist");
		}
	}

	void TriggerHighClapping() { 

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("fist pump animation")) {
			animator.SetTrigger ("T_raisedFistToHighClapper");
		}
	}

	
}
