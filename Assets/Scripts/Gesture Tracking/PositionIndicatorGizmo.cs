using UnityEngine;
using System.Collections;

public class PositionIndicatorGizmo : MonoBehaviour {

	public Color Color;
	public float Radius;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = Color;
		Gizmos.DrawSphere(transform.position, Radius);
	}
}
