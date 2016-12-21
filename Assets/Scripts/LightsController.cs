using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour
{

	public GameObject[] turnTheseOn;

	public void TurnOnMains ()
	{
		foreach (GameObject go in turnTheseOn) { 

			go.SetActive (true);
		}

	}


	public void TurnOffMains ()
	{
		foreach (GameObject go in turnTheseOn) { 

			go.SetActive (false);
		}
	}

//	// Use this for initialization
//	void Start ()
//	{
//		
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		
//	}
}
