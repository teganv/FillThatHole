using UnityEngine;
using System.Collections;

public class HoleSpace : MonoBehaviour {

	private Hole hole;
	private bool collidedThisFrame;

	// Use this for initialization
	void Start () {	
		hole = GetComponentInParent<Hole> ();
	}
	
	void OnTriggerStay2D(Collider2D coll) {
		if (coll.CompareTag ("Ground") && coll.GetComponentInParent<Rigidbody2D>().velocity == Vector2.zero && !collidedThisFrame) { //collidedThisFrame is there to prevent it getting called twice, once from each of the colliders on the tetronimo blocks
			collidedThisFrame = true;
			hole.filledArea++;
			Destroy (this);
		}
	}

	/*void OnTriggerExit2D(Collider2D coll) {
		if (coll.CompareTag ("Ground")) {
			print ("undoin it: " + coll.name);
			hole.filledArea--;
			print ("HoleSpace count: " + hole.filledArea.ToString () + "/" + hole.area.ToString ());
		}
	}*/
}
