using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroRunner : MonoBehaviour {
    public Image image1;
    public Image image2;
    public float timeToSwitch = 2.0f;
    //public Camera camera;

    public Color secondColor;
    private bool IsWaitingToLoadScene;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > timeToSwitch)
        {
            image1.gameObject.SetActive(false);
            Camera.main.backgroundColor = secondColor;
            image2.gameObject.SetActive(true);
            IsWaitingToLoadScene = true;

        }
        if (Time.time > timeToSwitch*2 &&IsWaitingToLoadScene)
        {
            Debug.Log("Splash time is over.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
