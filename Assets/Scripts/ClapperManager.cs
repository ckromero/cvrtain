using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapperManager : MonoBehaviour
{
	//clapping levels: murmurs, quiet, polite, medium, large, huge
	public ClapControl[] clapControls;
	public float clapAdjustedChance=0.3f;
	public bool IsMurmurs, IsQuiet, IsPolite, IsMedium, IsLarge, IsHuge;
	
	//	private List<ClapControl> clapControls;


	// Use this for initialization
	void Start ()
	{
//		foreach (GameObject go in clappers) { 
//			animators.Add (go.GetComponent<Animator> ());
//		}
//		foreach (ClapControl clapControl in clapControls) { 
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsMurmurs) {
			ChangeLevel (0);
			IsMurmurs = false;
		}
		if (IsQuiet) {
			ChangeLevel (1);
			IsQuiet = false;
		}
		if (IsPolite) {
			ChangeLevel (2);
			IsPolite = false;
		}
		if (IsMedium) {
			ChangeLevel (3);
			IsMedium = false;
		}
		if (IsLarge) {
			ChangeLevel (4);
			IsLarge = false;
		}
		if (IsHuge) {
			ChangeLevel (5);
			IsHuge = false;
		}
	}

	public void ChangeLevel (int levelNum)
	{ 

		Debug.Log ("Clap Manager called with levelNum" + levelNum);
		string targetTrigger = "";


		switch (levelNum) {
		case 0:
			targetTrigger = "triggerMildClapping";	
			break;	
		case 1:
			targetTrigger = "triggerMildClapping";	
			break;	
		case 2:
			targetTrigger = "triggerHighClapping";	
			break;
		case 3:
			targetTrigger = "triggerHighClapping";	
			break;
		case 4:
			targetTrigger = "triggerRaisedFist";	
			break;
		case 5:
			targetTrigger = "triggerRaisedFist";	
			break;
		}
		
		foreach (ClapControl clapControl in clapControls) {
			if (Random.value > clapAdjustedChance) {
				clapControl.WhichTrigger (targetTrigger);
			}
		}

//		public bool triggerRaisedFist = false;
//		public bool triggerHighClapping = false;
//		public bool triggerMildClapping = false;



	}
}
