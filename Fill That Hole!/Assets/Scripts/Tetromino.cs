using UnityEngine;
using System.Collections;

public class Tetromino : MonoBehaviour {

	private bool locked = false;
	private bool dropped = false;
	private int holeWidth;
	public Rigidbody2D rb;
	public int currentPosition = 0;
	private float lastFrameHorizontalAxisValue = 0, lastFrameDropAxisValue = 0;
	public GameObject particles, tetrominoPerson;
	private Hole hole;
	private bool alreadyCollided;

	private GameManager gameManager;

	private AudioSource source;
	private AudioSource dirtHitSource;

	private int gameMode = 0; //0 = choosy, 1 = random blocks

	public GameObject previewBlock;
	private GameObject preview;



	// Use this for initialization
	void Start () {
		UpdatePreviewBlock ();
		gameManager = GameObject.Find ("GameController").GetComponent<GameManager> ();
		gameMode = gameManager.GetGameMode();
		source = GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody2D> ();
		hole = gameManager.GetActiveHole().GetComponent<Hole>();
		holeWidth = hole.blockWidth;

		dirtHitSource = GameObject.Find ("GameController").GetComponent<AudioSource> ();
	}
	

	void Update () {
		if (!locked) {
			var axisValue = Input.GetAxisRaw("Horizontal");
			var horizontalAxisChange = axisValue - lastFrameHorizontalAxisValue;
			if (Mathf.Abs(horizontalAxisChange) > 0.7 && !dropped) {
				if (axisValue > 0 && currentPosition < holeWidth) {
					rb.MovePosition(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));
					currentPosition++;
				} else if (axisValue < 0 && currentPosition > -1) {
					rb.MovePosition(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
					currentPosition--;
				}
				UpdatePreviewBlock ();
			}
			lastFrameHorizontalAxisValue = axisValue;

			axisValue = Input.GetAxisRaw("Drop");
			var dropAxisChange = axisValue - lastFrameDropAxisValue;
			if (Mathf.Abs(dropAxisChange) > .6) {
				if (axisValue < 0) {
					StartCoroutine (drop ());
				}
			} 
			if (Input.GetButtonDown("Drop")) {
				StopAllCoroutines ();
				StartCoroutine (drop ());
			}
			lastFrameDropAxisValue = axisValue;

			if (Input.GetButtonDown ("Turn") && !dropped) {
				Vector3 rotation = new Vector3 (0, 0, -90f) * Input.GetAxisRaw ("Turn");
				transform.Rotate (rotation, Space.World);
				tetrominoPerson.GetComponent<Wiggler> ().UpdateRotationRanges (rotation); //this does nothing right now
				UpdatePreviewBlock ();
			}		
			if (Input.GetButtonDown("Shift")) {
				GameObject newTetromino = gameManager.spawnNextTetromino (new Vector3(hole.transform.position.x + currentPosition, 8f, 0f));
				newTetromino.transform.parent = gameManager.level.transform;
				newTetromino.transform.localRotation = transform.localRotation;
				newTetromino.GetComponent<Tetromino> ().currentPosition = currentPosition;
				Destroy(gameObject);
				Destroy (preview);
			}
		} else {
			GameObject newTetromino = gameManager.spawnSameTetromino (new Vector3 (hole.transform.position.x + currentPosition, 8f, 0f));
			newTetromino.transform.localRotation = transform.localRotation;
			newTetromino.GetComponent<Tetromino> ().currentPosition = currentPosition;
			newTetromino.transform.parent = gameManager.level.transform;
			dirtHitSource.Play ();
			Destroy (preview);
			Destroy (this);
		}
	}

	IEnumerator drop() {
		Destroy (preview);
		dropped = true;
		source.pitch = Random.Range (.85f, 1.15f);
		source.Play ();
		while (!locked) {
			rb.velocity = new Vector2 (0, -18f);
			yield return null;
		}
		yield break;
	}

	public void lockBlock() {
		locked = true;
		rb.velocity = Vector2.zero;
		transform.position = new Vector3 (transform.position.x,  Mathf.Ceil(transform.position.y), transform.position.z);
		particles.SetActive (true);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag("Ground") && !alreadyCollided && dropped) {
			alreadyCollided = true;
			Tetromino parentTetromino = GetComponentInParent<Tetromino> ();
			if (parentTetromino != null)
				parentTetromino.lockBlock ();
		}
	}

	private void UpdatePreviewBlock() {
		Destroy (preview);
		preview = Instantiate (previewBlock, new Vector3( transform.position.x, transform.position.y - 1, transform.position.z), transform.rotation) as GameObject;
		preview.transform.SetParent (transform);
	}

}
