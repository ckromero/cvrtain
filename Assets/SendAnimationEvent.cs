using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEvent : MonoBehaviour {
	public ExpManager expManager;
	private Animator animator;
//	public bool IsStartAnimation=false;

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
//	public void StartShow(){
//			expManager.StartShow ();
//	}
	public void SendCue(string cueName) { 
		expManager.SendCue (cueName);

	}

	public void StopAnimation() { 
		Debug.Log ("stopping animation");
		animator.speed=0;
//		animator.enabled=false;
	}
	public void StartAnimation() { 
		Debug.Log ("starting animation");
		animator.speed = 1;
//		animator.enabled = true;
	}
}
