using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClapperManager : MonoBehaviour
{
	//clapping levels: murmurs, quiet, polite, medium, large, huge
	public ClapControl[] clapControls;
	public float clapAdjustedChance=0.3f;
	public bool IsMurmurs, IsQuiet, IsPolite, IsMedium, IsLarge, IsHuge, IsAllQuiet;
	
	//	private List<ClapControl> clapControls;


	// Use this for initialization
	void Start ()
	{
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
		if (IsAllQuiet) {
			ChangeLevel (0,true);
			IsAllQuiet = false;
		}
	}

	public void ChangeLevel (int levelNum, bool all=false)
	{ 

		//Debug.Log ("Clap Manager called with levelNum" + levelNum);
		string targetTrigger = "";


		switch (levelNum) {
		case 0:
			targetTrigger = "triggerIdle";	
			break;	
		case 1:
			targetTrigger = "triggerIdle";	
			break;	
		case 2:
			targetTrigger = "triggerMildClapping";	
			break;
		case 3:
			targetTrigger = "triggerMildClapping";	
			break;
		case 4:
			targetTrigger = "triggerHighClapping";	
			break;
		case 5:
			targetTrigger = "triggerRaisedFist";	
			break;
		}
		
		foreach (ClapControl clapControl in clapControls) {
			if (all) {
				clapControl.WhichTrigger (targetTrigger);
			} else if (Random.value > clapAdjustedChance) {
				clapControl.WhichTrigger (targetTrigger);
			}
		}

	}
}
