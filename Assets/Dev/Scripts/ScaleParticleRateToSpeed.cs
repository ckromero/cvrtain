using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleParticleRateToSpeed : MonoBehaviour {

    Vector3 prev;
    float avgDist;
    public float particleAmountScale;
    public float particleLifetimeScale;
    public float minParticleLifetime;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float dist = Vector3.Distance(this.transform.position, prev);
        avgDist = avgDist * 10;
        avgDist += dist;
        avgDist /= 11;
        GetComponent<ParticleSystem>().emissionRate = avgDist*particleAmountScale;
        GetComponent<ParticleSystem>().startLifetime = Mathf.Max(minParticleLifetime, avgDist * particleLifetimeScale);
        prev = this.transform.position;
	}
}
