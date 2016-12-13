using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEvent : MonoBehaviour {
	public ExpManager expManager;
	private Animator animator;
	public bool IsStartAnimation=false;

	public void Start() {
		animator = GetComponent<Animator> ();

	}
	public void Update() {
		if (IsStartAnimation) {
			StartAnimation ();
			IsStartAnimation = false;
		}

	}
	public void CurtainOpened(){ 
			expManager.CurtainOpened ();
	}
	public void StartShow(){
			expManager.StartShow ();
	}
	public void StopAnimation() { 
		Debug.Log ("stopping animation");
		animator.enabled=false;
	}
	public void StartAnimation() { 
		Debug.Log ("starting animation");
		animator.enabled = true;

	}

}
