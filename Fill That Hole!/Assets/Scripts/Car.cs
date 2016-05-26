using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour {

	public float speed = 1.3f;
	private bool speedUp = false;
	private AudioSource source;
	private GameManager gameManager;
	private Rigidbody2D rb;

	private HealthManager healthManager;
	public GameObject particles;

	public bool alive = true;



	// Use this for initialization
	void Start () {
		healthManager = GameObject.Find ("HealthManager").GetComponent<HealthManager> ();
		gameManager = GameObject.Find ("GameController").GetComponent<GameManager>();
		source = GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update() {
		if (Input.GetButtonDown ("Vroom")) {
			speedUp = true;
			source.Play ();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (alive) {
			rb.velocity = new Vector2 (speed, rb.velocity.y);

			if (speedUp) {
				rb.velocity = new Vector2 (12f, rb.velocity.y);
			}
		}
			
	}


	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag ("Nexthole")) {
			speed += .12f;
			speedUp = true;
			healthManager.incrementHoles ();
			Destroy (coll.gameObject);
			//SceneManager.LoadScene (1);

			gameManager.CreateNewLevel ();
		}

		else if (coll.CompareTag ("Nextflatground") && !gameManager.game) {
			gameManager.CreateNewFlatGround ();
			Destroy (coll.gameObject);
		}

		else if (coll.CompareTag ("vroomCancel")) {
			speedUp = false;
			Destroy (coll.gameObject);
		}
	}


	public void Explode() {
		alive = false;
		particles.SetActive (true);
		rb.velocity = new Vector2 (0f, 40f);
	}

}
