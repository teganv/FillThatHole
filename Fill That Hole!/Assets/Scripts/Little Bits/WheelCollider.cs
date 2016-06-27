using UnityEngine;
using System.Collections;

public class WheelCollider : MonoBehaviour {

	Rigidbody2D parentRb;

	// Use this for initialization
	void Start () {
		parentRb = GetComponentInParent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//parentRb.AddForce (new Vector2(0, 500f));
		parentRb.angularVelocity = 0;
		parentRb.velocity = new Vector2 (parentRb.velocity.x, 0);
	}
}
