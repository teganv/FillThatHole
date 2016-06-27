using UnityEngine;
using System.Collections;

public class BumpChecker : MonoBehaviour {

	private Hole hole;

	// Use this for initialization
	void Start () {	
		hole = GetComponentInParent<Hole> ();
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (coll.CompareTag ("Ground") && coll.GetComponentInParent<Rigidbody2D> ().velocity == Vector2.zero) {
			hole.MadeBump ();
			Destroy (this);
		}
	}


}
