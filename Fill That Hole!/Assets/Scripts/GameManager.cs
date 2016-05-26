using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GameObject holeSpace; //the sprites that fill the hole
	public GameObject gridSpace; //transparent square to show grid in sky above hole
	public GameObject ground; //the blocks that go at the bottom of the hole
	public GameObject endTrigger, vroomCancelTrigger;
	public GameObject flatGround;

	public GameObject[] clouds;
	public GameObject[] tetrominos;
	public GameObject[] tetrominoPreviews;
	public GameObject car;
	public GameObject level = null; //holds the previous hole and all associated roads/tetrominos
	public float roadHeight = -2f;

	private int tetrominoIndex = 0;
	private int tetrominoPreviewIndex = 1;

	public bool game = false;

	public GameObject groundLine;
	public GameObject fillSquadStartAnimation;
	private float potHoleStart; //note: changing this in the editor will do nothing
	private float startPoint; //Where the previous level ended and where the next will begin

	private GameObject activeHole; //the current hole
	private GameObject prevLevel; //keeps track of previous levels for deleting once they're off screen

	private MusicManager musicManager;
	private HealthManager healthManager;


	void Start() {
		//PlayerPrefs.DeleteAll ();
		//for(int i = 0; i < 10; i ++) {
		//	PlayerPrefs.SetString ("score" + (i + 1).ToString () + "name", "Aspiring Hole Filler");
		//	PlayerPrefs.GetInt ("score" + (i + 1).ToString (), 0);
		//}
		startPoint = 12;
		potHoleStart = startPoint + 24;
		musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		healthManager = GameObject.Find ("HealthManager").GetComponent<HealthManager> ();

		int numClouds = Random.Range (3, 8);

		for (int i = 0; i < numClouds; i++) {
			Instantiate (clouds [Random.Range (0, clouds.Length - 1)], new Vector3 (Random.Range (-35, 30f), Random.Range (0f, 10f), Random.Range(5, 15)), Quaternion.Euler(new Vector3(270f, 0, 0)));
		}
	}

	void Update() {
		if (Input.GetButtonDown ("Exit")) {
			HealthManager healthManager = GameObject.Find ("HealthManager").GetComponent<HealthManager> ();
			//healthManager.resetHolesCleared ();
			//healthManager.resetHealth ();
			SceneManager.LoadScene ("Main");
		} else if (Input.GetButtonDown ("Vroom") && !game) {
			fillSquadStartAnimation.SetActive (true);
			musicManager.playChant ();
			SetStartPoints (-20);
			game = true;
			Instantiate (vroomCancelTrigger, new Vector3 (startPoint + 2, roadHeight, 0), Quaternion.identity);
			CreateNewLevel ();
		}
	}

	private void SetStartPoints(int distance) {
		startPoint += distance;
		potHoleStart = startPoint + 29f;
	}
		

	//creates new flat ground for the car to ride on before the game starts. Includes a trigger that will spawn more.
	public void CreateNewFlatGround() {
		print ("making new ground");
		Instantiate (flatGround, new Vector3 (startPoint, roadHeight, 0), Quaternion.identity);
		SetStartPoints (12);

	}
		
	//creates the next level complete with roads, a hole, and a trigger after the hole that spawns the next one
	public void CreateNewLevel() {
		fillSquadStartAnimation.SetActive (true);
		fillSquadStartAnimation.GetComponent<FillSquadStartAnimation> ().FlyAnimation ();

		Destroy (prevLevel);
		prevLevel = level;
		print ("Creating next hole");
		foreach (Tetromino tetromino in FindObjectsOfType<Tetromino>()) {
			Destroy (tetromino);
		}

		level = new GameObject ("Level" + healthManager.getHolesCleared ().ToString ()); 
		Instantiate (vroomCancelTrigger, new Vector3 (startPoint + 2, roadHeight, 0), Quaternion.identity);

		GameObject groundBeforeHole = Instantiate (groundLine);
		groundBeforeHole.transform.parent = level.transform;
		groundBeforeHole.name = "beforeHoleGround";
		GameObject groundAfterHole = Instantiate (groundLine);
		groundAfterHole.transform.parent = level.transform;
		groundAfterHole.name = "afterHoleGround";

		drawLine (new Vector3 (startPoint, roadHeight, 0), new Vector3 (potHoleStart-.05f, roadHeight, 0), groundBeforeHole);
		GameObject hole = spawnHole();
		hole.transform.parent = level.transform;
		float holeEndX = potHoleStart + hole.GetComponent<Hole> ().blockWidth;
		drawLine (new Vector3 (holeEndX, roadHeight, 0), new Vector3 (holeEndX + 13, roadHeight, 0), groundAfterHole);
		Camera.main.GetComponent<CameraTargetHole> ().SetTarget (hole.transform.position);

		GameObject trigger = Instantiate (endTrigger, new Vector3(holeEndX + 7, roadHeight, 0), Quaternion.identity) as GameObject;
		trigger.transform.parent = level.transform;
		GameObject startingTetromino = spawnSameTetromino (new Vector3 (hole.transform.position.x, 8f, 0f));
		startingTetromino.transform.parent = level.transform;

		int numClouds = Random.Range (3, 9);

		for (int i = 0; i < numClouds; i++) {
			Instantiate (clouds [Random.Range (0, clouds.Length - 1)], new Vector3 (Random.Range (startPoint + 5, startPoint + 60), Random.Range (0f, 10f), Random.Range(5, 15)), Quaternion.Euler(new Vector3(270f, 0, 0)));
		}

		SetStartPoints (46);
	}



	//draws a line (from a stretched cube) between lineStart and lineEnd
	private void drawLine (Vector3 lineStart, Vector3 lineEnd, GameObject line)
	{
		var posC = ((lineEnd - lineStart) * .5f) + lineStart;
		posC = new Vector3 (posC.x, posC.y, 0);
		var lengthC = (lineEnd - lineStart).magnitude;
		var sineC = (lineEnd.y - lineStart.y) / lengthC;
		var angleC = Mathf.Asin (sineC) * Mathf.Rad2Deg;
		if (lineEnd.x < lineStart.x) {
			angleC = 0 - angleC;
		}
		line.transform.position = posC;
		line.transform.localScale = new Vector3 (lengthC, 1f, 5f); //5f is the depth of the block
		line.transform.rotation = Quaternion.Euler (0, 0, angleC);
	}

	public GameObject spawnNextTetromino(Vector3 pos) {
		tetrominoIndex++;
		if (tetrominoIndex >= tetrominos.Length)
			tetrominoIndex = 0;
		GameObject newTetromino = Instantiate (tetrominos [tetrominoIndex], pos, Quaternion.identity) as GameObject;
		return newTetromino;
	}

	public GameObject spawnSameTetromino(Vector3 pos) {
		GameObject newTetromino = Instantiate (tetrominos [tetrominoIndex], pos, Quaternion.identity) as GameObject;
		return newTetromino;
	}


	public GameObject spawnHole() {
		GameObject hole = new GameObject("Hole");
		GameObject grid = new GameObject ("Grid");
		grid.transform.parent = hole.transform;
		hole.tag = "Hole";
		hole.transform.position = new Vector3(potHoleStart+.5f, roadHeight, 0f);
		Hole holeScript = hole.AddComponent<Hole> ();
		hole.AddComponent<MeshRenderer> ();
		int width = Random.Range (4, 10);
		holeScript.blockWidth = width;
		for (int i = 0; i < width; i++) {
			for (int k = 0; k < 15; k++) {
				GameObject gridSquare = Instantiate (gridSpace, new Vector3 (hole.transform.position.x + i, hole.transform.position.y + 1 + k, 1), Quaternion.identity) as GameObject;
				gridSquare.transform.parent = grid.transform;
			}
			int depth = Random.Range (2, 7);
			for (int j = 0; j < depth; j++) {
				GameObject holeSquare = Instantiate(holeSpace, new Vector3(hole.transform.position.x + i, hole.transform.position.y - j, 1), Quaternion.identity) as GameObject;
				holeSquare.transform.parent = hole.transform;
			}
			for (int k = depth; k < 7; k++) {
				GameObject groundSquare = Instantiate (ground, new Vector3 (hole.transform.position.x + i, hole.transform.position.y - k, 0), Quaternion.identity) as GameObject;
				groundSquare.transform.parent = hole.transform;
			}
			for (int l = 1; l < 7; l++) {
				GameObject groundSquare = Instantiate (ground, new Vector3 (hole.transform.position.x + width, hole.transform.position.y - l, 0), Quaternion.identity) as GameObject;
				groundSquare.transform.parent = hole.transform;
			}
		}
		activeHole = hole;
		return hole;
	}

	public GameObject GetActiveHole() {
		return activeHole;
	}
}
