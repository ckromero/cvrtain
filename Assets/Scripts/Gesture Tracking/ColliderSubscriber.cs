using UnityEngine;
using System.Collections;

public class ColliderSubscriber : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnColliderEnter(Collision collision){
		OnColliderEnterDelegate(collision);
	}

	void OnColliderExit(Collision collision){
		OnColliderExitDelegate(collision);
	}

	void OnTriggerEnter(Collider other){
		OnTriggerEnterDelegate(other);
	}

	void OnTriggerExit(Collider other){
		OnTriggerExitDelegate(other);
	}

	public delegate void onColliderEnterDelegate(Collision collision);
	public event onColliderEnterDelegate OnColliderEnterDelegate;
	public delegate void onColliderExitDelegate(Collision collision);
	public event onColliderExitDelegate OnColliderExitDelegate;
	public delegate void onTriggerEnterDelegate(Collider other);
	public event onTriggerEnterDelegate OnTriggerEnterDelegate;
	public delegate void onTriggerExitDelegate(Collider other);
	public event onTriggerExitDelegate OnTriggerExitDelegate;
}
