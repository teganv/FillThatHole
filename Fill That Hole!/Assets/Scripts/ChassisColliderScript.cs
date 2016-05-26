using UnityEngine;
using System.Collections;

public class ChassisColliderScript : MonoBehaviour {

	private Rigidbody2D parentRb;
	public AudioClip[] crashClips;
	private AudioSource audioSource;
	private HealthManager healthManager;


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		healthManager = GameObject.Find ("HealthManager").GetComponent<HealthManager> ();
		parentRb = GetComponentInParent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D coll) {
		audioSource.pitch = Random.Range (.85f, 1.1f);
		parentRb.AddForce (new Vector2(0, 500f));
		healthManager.loseHealth(coll.relativeVelocity.magnitude);
		audioSource.PlayOneShot(crashClips[Random.Range(0, crashClips.Length - 1)]);
	}
}
