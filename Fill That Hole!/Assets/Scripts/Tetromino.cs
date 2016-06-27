using UnityEngine;
using System.Collections;

public class Tetromino : MonoBehaviour {

	private bool locked = false;
	private bool dropped = false;
	private int holeWidth;
	public Rigidbody2D rb;
	public int currentPosition = 0;
	public GameObject particles, tetrominoPerson;
	private Hole hole;

	private GameManager gameManager;

	private AudioSource source;
	public AudioSource dirtHitSource;

	private int gameMode = 0; //0 = choosy, 1 = random blocks

	public GameObject previewBlock;
	private GameObject preview;

	private bool ableToMoveLeft = true, ableToMoveRight = true, ableToTurn = true;
	private Coroutine moveLeftReset, moveRightReset;



	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameController").GetComponent<GameManager> ();
		gameMode = gameManager.GetGameMode();
		source = GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody2D> ();
		hole = gameManager.GetActiveHole().GetComponent<Hole>();
		holeWidth = hole.blockWidth;

		dirtHitSource = gameManager.GetComponent<AudioSource> ();

		UpdatePreviewBlock ();
	}


	void Update () {
		if (!locked) {
			if (!dropped) {
				var horizontal = Input.GetAxis ("Horizontal");
				if (horizontal == 0) {
					ableToMoveLeft = true;
					ableToMoveRight = true;
				} else {
					if (horizontal > 0 && currentPosition < holeWidth && ableToMoveRight) {
						MoveRight ();
						ableToMoveRight = false;
						UpdatePreviewBlock ();
						if (moveRightReset != null)
							StopCoroutine (moveRightReset);
						moveRightReset = StartCoroutine (ResetAbleToMoveRight ());
					} else if (horizontal < 0 && currentPosition > -1 && ableToMoveLeft) {
						MoveLeft ();
						ableToMoveLeft = false;
						UpdatePreviewBlock ();
						if (moveLeftReset != null)
							StopCoroutine (moveLeftReset);
						moveLeftReset =	StartCoroutine (ResetAbleToMoveLeft ());
					}
					UpdatePreviewBlock ();
				}
					
				var DropAxis = Input.GetAxis ("Drop");
				if (Mathf.Abs (DropAxis) > .7f) {
					ableToTurn = false;
					StopAllCoroutines ();
					Drop ();
				} 

				var turnAxis = Input.GetAxisRaw ("Turn");
				if (turnAxis == 0)
					ableToTurn = true;
				else if (Mathf.Abs(turnAxis) > .7f && ableToTurn) {
					ableToTurn = false;
					Vector3 rotation = new Vector3 (0, 0, -90f) * turnAxis;
					transform.Rotate (rotation, Space.World);
					tetrominoPerson.GetComponent<Wiggler> ().UpdateRotationRanges (rotation); //this does nothing right now
					UpdatePreviewBlock ();
				}		
				if (Input.GetButtonDown ("Shift")) {
					GameObject newTetromino = gameManager.SpawnNextTetromino (new Vector3 (hole.transform.position.x + currentPosition, 8f, 0f));
					newTetromino.transform.parent = gameManager.level.transform;
					newTetromino.transform.localRotation = transform.localRotation;
					newTetromino.GetComponent<Tetromino> ().currentPosition = currentPosition;
					Destroy (gameObject);
					Destroy (preview);
				}
			}
		} else {
			GameObject newTetromino = gameManager.SpawnSameTetromino (new Vector3 (hole.transform.position.x + currentPosition, 8f, 0f));
			newTetromino.transform.localRotation = transform.localRotation;
			newTetromino.GetComponent<Tetromino> ().currentPosition = currentPosition;
			newTetromino.transform.parent = gameManager.level.transform;
			dirtHitSource.Play ();
			Destroy (preview);
			Destroy (this);
		}
	}

	private void MoveLeft() {
		rb.MovePosition(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));
		currentPosition--;
	}

	private void MoveRight() {
		rb.MovePosition(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));
		currentPosition++;
	}

	private IEnumerator ResetAbleToMoveLeft() {
		yield return new WaitForSeconds (.2f);
		ableToMoveLeft = true;
	}

	private IEnumerator ResetAbleToMoveRight() {
		yield return new WaitForSeconds (.2f);
		ableToMoveRight = true;
	}

	private void Drop() {
		Destroy (preview);
		dropped = true;
		source.pitch = Random.Range (.85f, 1.15f);
		source.Play ();
		rb.velocity = new Vector2 (0, -20f);
		return;
	}

	public void LockBlock() {
		locked = true;
		rb.velocity = Vector2.zero;
		transform.position = new Vector3 (transform.position.x,  Mathf.Ceil(transform.position.y), transform.position.z);
		particles.SetActive (true);
		hole.UpdateGameManagerPercentFilled ();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag("Ground") && dropped) {
			LockBlock ();
		}
	}

	private void UpdatePreviewBlock() {
		Destroy (preview);
		preview = Instantiate (previewBlock, new Vector3(rb.position.x, rb.position.y, 0), transform.rotation) as GameObject;
		preview.transform.SetParent (transform);
		return;
	}

	public bool IsLocked() {
		return locked;
	}

}
