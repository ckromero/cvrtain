using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour
{
	public GameObject[] turnTheseOn;
	public GameObject HouseLights;
	private Light houseLight;
	private bool rollUpHouseLights = false;
	public float targetHouseLightIntensity = 0.3f;

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

	public void HouseToHalf ()
	{
		if (!rollUpHouseLights) {
            rollUpHouseLights = true;
			houseLight.intensity = 0.0f;
			HouseLights.SetActive (true);
            Debug.Log("Rolluphouselights is false");
		}else
        {
            Debug.Log("Rolluphouselights is true");
        }
	}

	public void HouseOff ()
	{
		HouseLights.SetActive (false);
	}

	void Start ()
	{
		houseLight = HouseLights.GetComponent<Light> ();
	}

	void Update ()
	{
		if (rollUpHouseLights) {
			//Debug.Log ("Rolling up house lights");
			if (houseLight.intensity < targetHouseLightIntensity) {			
				houseLight.intensity += 0.2f * Time.deltaTime;
			} else {
				rollUpHouseLights = false;
			}
		}
	}
}
