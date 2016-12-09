using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAnimationEvent : MonoBehaviour {
	public ExpManager expManager;

	public void CurtainOpened(){ 
			expManager.CurtainOpened ();
	}
	public void StartShow(){
			expManager.StartShow ();
	}

}
