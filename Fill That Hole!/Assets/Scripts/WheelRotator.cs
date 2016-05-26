using UnityEngine;
using System.Collections;

public class WheelRotator : MonoBehaviour {

	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rb.AddTorque (-5f);
	}
}
