using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEvent : MonoBehaviour {
	public ExpManager expManager;
	private Animator animator;
//	public bool IsStartAnimation=false;
	public bool IsRollingBack=false;
	private int lastCheckedPoint=0;
	private IEnumerator coroutine;
	public float HoldAnimationSeconds = 4.0f;


	public void Start() {
		animator = GetComponent<Animator> ();

	}
	public void Update() {
//		if (IsStartAnimation) {
//			StartAnimation ();
//			IsStartAnimation = false;
//		}

	}
	public void CurtainOpened(){ 
			expManager.SendCue("CurtainOpened");
	}

	public void SendCue(string cueName) { 
		expManager.SendCue (cueName);
	}

	public void EnableIsRollingBack() {
		IsRollingBack = true;
	}

	public void  CheckRollBack(int whichRollBack){
		Debug.Log ("CheckRollBack " + whichRollBack + " called");
		if (whichRollBack > lastCheckedPoint&&IsRollingBack) {
//			IsRollingBack = true;
//			animator.speed = -1.0f;

			StopAnimation();
			coroutine = HoldAnimation ();
			StartCoroutine (coroutine);
			lastCheckedPoint = whichRollBack;
		}
	}

	private void StopRollBack(){
		if(IsRollingBack) { 
			StopAnimation();
			coroutine = HoldAnimation ();
			StartCoroutine (coroutine);
		}
	}

	IEnumerator	 HoldAnimation(){
		Debug.Log ("Running HoldAnimation");
		StopAnimation ();
		yield return new WaitForSeconds (HoldAnimationSeconds);
		StartAnimation ();
		IsRollingBack = false;
	}

	public void StopAnimation() { 
		Debug.Log ("stopping animation");
		animator.speed=0;
//		animator.enabled=false;
	}

	public void StartAnimation() { 
		Debug.Log (this.gameObject.name + ": starting animation");
		animator.speed = 1;
//		animator.enabled = true;
	}

	public void ResetAnimation() { 
		Debug.Log("ResetAnimation received");
//		animator.SetTime (0.0f);
		//reset to beginning of clip
		animator.Play("",-1,0.0f);
//		animator.speed = 1;
	}
}
