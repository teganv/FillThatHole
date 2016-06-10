using UnityEngine;
using System.Collections;

public class PreviewBlock : MonoBehaviour {

	public bool hitBottom = false;
	private Rigidbody2D rb;
	private MeshRenderer[] renderers;


	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		renderers = GetComponentsInChildren<MeshRenderer> ();
		//rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

	}

	void Update () {
		if (!hitBottom) {
			rb.MovePosition (new Vector2(transform.position.x, transform.position.y - .99f));
		}
	}
	

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag ("Ground") && coll.transform.parent != transform.parent) {
			StartCoroutine(BlinkOn());
			rb.isKinematic = true;
			hitBottom = true;
			rb.velocity = Vector3.zero;
			transform.position = new Vector3 (transform.position.x, Mathf.Ceil (transform.position.y), transform.position.z);
		}
	}

	private IEnumerator BlinkOn() {
		foreach (MeshRenderer rndr in renderers)
			rndr.enabled = true;

		yield return new WaitForSeconds (.4f);
		StartCoroutine (BlinkOff ());
	
	}

	private IEnumerator BlinkOff() {
		foreach (MeshRenderer rndr in renderers)
			rndr.enabled = false;

		yield return new WaitForSeconds (.4f);
		StartCoroutine (BlinkOn ());

	}
}
