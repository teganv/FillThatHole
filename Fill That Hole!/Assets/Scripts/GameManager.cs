using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject holeSpace; //the sprites that fill the hole
	public GameObject gridSpace; //transparent square to show grid in sky above hole
	public GameObject ground; //the blocks that go at the bottom of the hole
	public GameObject endTrigger, vroomCancelTrigger, tookDamageThisHoleResetTrigger;
	public GameObject flatGround;
	public GameObject titleLogo;
	private TitleLogo logoScript;
	public GameObject healthBar;
	public GameObject speedSign;
	public GameObject controlsText;
	public Text percentFilledText;
	public Animator GoodFillTextParent, GoodFillTextChild;


	public GameObject[] clouds;
	public GameObject[] tetrominos;
	public GameObject[] tetrominoPreviews;
	public GameObject car;
	public GameObject level = null; //holds the previous hole and all associated roads/tetrominos
	public GameObject bumpCheck; //trigger that checks if a hole has been overfilled (thus preventing perfect fill bonuses)

	public float roadHeight = -2f;

	private int tetrominoIndex = 0;
	private int tetrominoPreviewIndex = 1;

	public bool game = false;

	public GameObject groundLine;
	public GameObject fillSquadStartAnimation;
	private float potHoleStart;
	private float startPoint; //Where the previous level ended and where the next will begin

	private GameObject activeHole; //the current hole
	private Hole activeHoleScript;
	private GameObject prevLevel; //keeps track of previous levels for deleting once they're off screen

	private MusicManager musicManager;
	private HealthManager healthManager;

	private float percent;

	private int gameMode = 0; //0 = choosy, 1 = random


	void Start() {
		gameMode = PlayerPrefs.GetInt ("GameMode");

		//Uncomment to reset high scores
		//PlayerPrefs.DeleteAll ();
		//for(int i = 0; i < 10; i ++) {
		//	PlayerPrefs.SetString ("score" + (i + 1).ToString () + "name", "Aspiring Hole Filler");
		//	PlayerPrefs.GetInt ("score" + (i + 1).ToString (), 0);
		//}
	
		logoScript = titleLogo.GetComponent<TitleLogo> ();
		startPoint = 12;
		potHoleStart = startPoint + 24;
		musicManager = GameObject.Find ("MusicManager").GetComponent<MusicManager> ();
		healthManager = GameObject.Find ("HealthManager").GetComponent<HealthManager> ();

		int numClouds = Random.Range (3, 8);

		for (int i = 0; i < numClouds; i++) {
			GameObject cloud = (GameObject) Instantiate (clouds [Random.Range (0, clouds.Length - 1)], new Vector3 (Random.Range (-35, 30f), Random.Range (2f, 10f), Random.Range(13, 20)), Quaternion.Euler(new Vector3(270f, 0, 0)));
			cloud.transform.localScale = cloud.transform.localScale * Random.Range (.5f, 1.05f);
		}
	}

	void Update() {
		if (Input.GetButtonDown ("Exit")) {
			SceneManager.LoadScene ("Main");
		} else if (Input.GetButtonDown ("Vroom") && !game) {
			controlsText.SetActive (false);
			healthBar.SetActive (true);
			logoScript.GetComponent<Animator>().SetBool("spin", true);
			fillSquadStartAnimation.SetActive (true);
			musicManager.playChant ();
			SetStartPoints (-20);
			game = true;
			Instantiate (vroomCancelTrigger, new Vector3 (startPoint + 2, roadHeight, 0), Quaternion.identity);
			CreateNewLevel ();
		}
		if (Input.GetButtonDown ("Shift") && !game) {
			SceneManager.LoadScene ("KeyboardControls");
		}
	}

	private void SetStartPoints(int distance) {
		startPoint += distance;
		potHoleStart = startPoint + 29f;
	}
		

	//creates new flat ground for the car to ride on before the game starts. Includes a trigger that will spawn more.
	public void CreateNewFlatGround() {
		int numClouds = Random.Range (3, 6);
		for (int i = 0; i < numClouds; i++) {
			GameObject cloud = (GameObject) Instantiate (clouds [Random.Range (0, clouds.Length - 1)], new Vector3 (Random.Range (startPoint, startPoint + 20), Random.Range (2f, 10f), Random.Range(13, 20)), Quaternion.Euler(new Vector3(270f, 0, 0)));
			cloud.transform.localScale = cloud.transform.localScale * Random.Range (.5f, 1.05f);
		}
		Instantiate (flatGround, new Vector3 (startPoint, roadHeight, 0), Quaternion.identity);
		SetStartPoints (12);

	}
		
	//creates the next level complete with roads, a hole, and a trigger after the hole that spawns the next one
	public void CreateNewLevel() {
		fillSquadStartAnimation.SetActive (true);
		fillSquadStartAnimation.GetComponent<FillSquadStartAnimation> ().FlyAnimation ();

		Destroy (prevLevel);
		prevLevel = level;

		foreach (Tetromino tetromino in FindObjectsOfType<Tetromino>()) {
			Destroy (tetromino);
		}

		level = new GameObject ("Level" + healthManager.getHolesCleared ().ToString ()); 
		Instantiate (vroomCancelTrigger, new Vector3 (startPoint + 2, roadHeight, 0), Quaternion.identity);
		GameObject sign = Instantiate (speedSign, new Vector3(startPoint + 15, roadHeight, 10), Quaternion.Euler(0, 0, Random.Range(-15, 15))) as GameObject;
		sign.GetComponentsInChildren<TextMesh> () [1].text = (healthManager.getHolesCleared () + 1).ToString();
		sign.transform.parent = level.transform;

		GameObject groundBeforeHole = Instantiate (groundLine);
		groundBeforeHole.transform.parent = level.transform;
		groundBeforeHole.name = "beforeHoleGround";
		GameObject groundAfterHole = Instantiate (groundLine);
		groundAfterHole.transform.parent = level.transform;
		groundAfterHole.name = "afterHoleGround";

		DrawLine (new Vector3 (startPoint, roadHeight, -.6f), new Vector3 (potHoleStart, roadHeight, -.6f), groundBeforeHole);
		GameObject hole = SpawnHole();
		hole.transform.parent = level.transform;
		float holeEndX = potHoleStart + hole.GetComponent<Hole> ().blockWidth;
		DrawLine (new Vector3 (holeEndX, roadHeight, -.6f), new Vector3 (holeEndX + 13, roadHeight, -.6f), groundAfterHole);
		Camera.main.GetComponent<CameraTargetHole> ().SetTarget (hole.transform.position);

		GameObject trigger = Instantiate (endTrigger, new Vector3(holeEndX + 7, roadHeight, 0), Quaternion.identity) as GameObject;
		trigger.transform.parent = level.transform;
		GameObject startingTetromino = SpawnSameTetromino (new Vector3 (hole.transform.position.x, 8f, 0f));
		startingTetromino.transform.parent = level.transform;

		int numClouds = Random.Range (3, 9);

		for (int i = 0; i < numClouds; i++) {
			GameObject cloud = (GameObject) Instantiate (clouds [Random.Range (0, clouds.Length - 1)], new Vector3 (Random.Range (startPoint + 5, startPoint + 60), Random.Range (2f, 10f), Random.Range(13, 20)), Quaternion.Euler(new Vector3(270f, 0, 0)));
			cloud.transform.localScale = cloud.transform.localScale * Random.Range (.5f, 1.05f);
		}

		SetStartPoints (46);
	}



	//draws a line (from a stretched cube) between lineStart and lineEnd
	private void DrawLine (Vector3 lineStart, Vector3 lineEnd, GameObject line)
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
		line.transform.localScale = new Vector3 (lengthC, 1f, 10f); //16f is the z depth of the block
		line.transform.rotation = Quaternion.Euler (0, 0, angleC);
	}

	public GameObject SpawnNextTetromino(Vector3 pos) {
		tetrominoIndex++;
		if (tetrominoIndex >= tetrominos.Length)
			tetrominoIndex = 0;
		GameObject newTetromino = Instantiate (tetrominos [tetrominoIndex], pos, Quaternion.identity) as GameObject;
		return newTetromino;
	}

	public GameObject SpawnSameTetromino(Vector3 pos) {
		GameObject newTetromino = Instantiate (tetrominos [tetrominoIndex], pos, Quaternion.identity) as GameObject;
		return newTetromino;
	}


	public GameObject SpawnHole() {
		Instantiate (tookDamageThisHoleResetTrigger, new Vector3(potHoleStart, roadHeight, 0f), Quaternion.identity);
		int holesCleared = healthManager.getHolesCleared ();
		int levelsClearedLog = 1;
		if (holesCleared != 0)
			levelsClearedLog = Mathf.CeilToInt(Mathf.Log(holesCleared, 2f));
		GameObject hole = new GameObject("Hole");
		GameObject grid = new GameObject ("Grid");
		grid.transform.parent = hole.transform;
		hole.tag = "Hole";
		hole.transform.position = new Vector3(potHoleStart+.5f, roadHeight, 0f);
		activeHoleScript = hole.AddComponent<Hole> ();
		hole.AddComponent<MeshRenderer> ();
		int width = Random.Range (4 + levelsClearedLog, 5 + levelsClearedLog);
		activeHoleScript.blockWidth = width;
		for (int i = 0; i < width; i++) {
			GameObject bumpChk = Instantiate (bumpCheck, new Vector3 (hole.transform.position.x + i, hole.transform.position.y + 1, .5f), Quaternion.identity) as GameObject;
			bumpChk.transform.parent = hole.transform;
			for (int k = 0; k < 15; k++) {
				GameObject gridSquare = Instantiate (gridSpace, new Vector3 (hole.transform.position.x + i, hole.transform.position.y + 1 + k, .5f), Quaternion.identity) as GameObject;
				gridSquare.transform.parent = grid.transform;
			}
			int depth = Random.Range (2, 6);
			for (int j = 0; j < depth; j++) {
				activeHoleScript.area++;
				GameObject holeSquare = Instantiate(holeSpace, new Vector3(hole.transform.position.x + i, hole.transform.position.y - j, .5f), Quaternion.identity) as GameObject;
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

	public int GetGameMode() {
		return gameMode;
	}

	public void UpdatePercentFilled() {
		StartCoroutine (UpdatePercentFilledCoroutine ());
	}

	private IEnumerator UpdatePercentFilledCoroutine() {
		yield return 0;
		//print ("GameManager count: " + activeHoleScript.filledArea.ToString () + "/" + activeHoleScript.area.ToString ());
		percent = (float) 100 * ((float) activeHoleScript.filledArea / (float) activeHoleScript.area);
		percentFilledText.text = percent.ToString("F1") + "% Filled";
		if (activeHoleScript.hasBump) {
			percentFilledText.text = "OVERFILLED";
		}
	}

	public void DisplayGoodFillText() {
		if (percent >= 70 && !healthManager.tookDamageThisHole) {
			GoodFillTextParent.gameObject.SetActive (true);
			GoodFillTextParent.gameObject.GetComponent<Text> ().text = "Good Fill!";
			GoodFillTextChild.gameObject.GetComponent<Text> ().text = "Good Fill!";
			healthManager.GainHealth (10);
			if (percent >= 80) {
				GoodFillTextParent.gameObject.GetComponent<Text> ().text = "GREAT Fill!";
				GoodFillTextChild.gameObject.GetComponent<Text> ().text = "GREAT Fill!";
				healthManager.GainHealth (10);
				if (percent >= 90) {
					GoodFillTextParent.gameObject.GetComponent<Text> ().text = "AWESOME FILL!";
					GoodFillTextChild.gameObject.GetComponent<Text> ().text = "AWESOME FILL!";
					healthManager.GainHealth (10);
					if (percent == 100) {
						GoodFillTextParent.gameObject.GetComponent<Text> ().text = "P E R F E C T  F I L L";
						GoodFillTextChild.gameObject.GetComponent<Text> ().text = "P E R F E C T  F I L L";
						healthManager.GainHealth (20);
					}
				}
			}
			GoodFillTextParent.gameObject.transform.rotation = Quaternion.identity;
			GoodFillTextParent.Play ("GoodFillTextParent", -1, .01f);
			GoodFillTextChild.Play ("GoodFillTextChild", -1, .01f);
		}
		print ("reset took damage this hole");
		healthManager.tookDamageThisHole = false;

	}


}
