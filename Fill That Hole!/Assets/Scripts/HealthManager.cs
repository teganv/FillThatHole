using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	public float carHealth = 400f;
	public GameObject healthBar;

	public int holesCleared = 0;

	private Vector3 cameraStartRotation;
	private float healthBarStartingXScale;

	public Text holesClearedText;

	/*public static HealthManager instance = null;

	public void Awake()
	{
		if (instance == null) {
			instance = this;
		}
		else if (instance != this)
			Destroy (gameObject);
	}*/

	void Start () {
		healthBar = GameObject.Find ("HealthBar");
		healthBarStartingXScale = healthBar.transform.localScale.x;
		cameraStartRotation = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		UpdateHealthBar ();

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void loseHealth(float healthLost) {
		carHealth -= healthLost;
		UpdateHealthBar ();
		if (carHealth <= 0f) {
			carHealth = 400;
			PlayerPrefs.SetInt ("Holes Cleared", holesCleared);
			SceneManager.LoadScene (2);
			UpdateHighScores ();
		}
		//Shake health bar
		StartCoroutine(shakeHealthBar(healthLost));
	}

	public void UpdateHighScores() {
		var highScoreFound = false;
		//iterate through PlayerPrefs high scores
		for (int i = 0; i < 10; i++) {
			if (holesCleared > PlayerPrefs.GetInt ("score" + (i + 1).ToString ())) {
				if (!highScoreFound) {
					//if this score is higher than one of them, this score is that new high score and set PlayerPrefs "newScore" to that high score position 
					string[] curNames = new string[11];
					int[] curScores = new int[11];
					for (int j = i; j < 10; j++) {
						curScores [j + 1] = PlayerPrefs.GetInt (("score" + (j + 1).ToString()));
						curNames[j + 1] = PlayerPrefs.GetString(("score" + (j + 1).ToString () + "name"));
					}
					for (int j = i; j < 10; j++) {
						PlayerPrefs.SetInt ("score" + (j + 2).ToString (), curScores [j + 1]);
						PlayerPrefs.SetString ("score" + (j + 2).ToString () + "name", curNames[j + 1]);
					}
					PlayerPrefs.SetInt ("score" + (i + 1).ToString (), holesCleared);
					PlayerPrefs.SetInt ("newScore", (i + 1)); 	//(this will be used to put the name in the right spot on the name entry screen)
					SceneManager.LoadScene("HighScoreEntry");
					highScoreFound = true;
					break;
				}
			}
		}
		holesCleared = 0;
	}

	public void UpdateHealthBar() {
		healthBar.transform.localScale = new Vector3 (Mathf.Max(0, carHealth / 400), 1, 1); //takes the max of zero and the value so the health bar won't go negative
	}

	public void incrementHoles() {
		holesCleared++;
		holesClearedText.text = holesCleared.ToString();
	}

	public int getHolesCleared() {
		return holesCleared;
	}

	public void resetHolesCleared() {
		holesCleared = 0;
	}

	public void resetHealth() {
		carHealth = 400f;
	}

	private IEnumerator shakeHealthBar(float healthLost) {
		int counter = 0;
		float inverseCameraRotation = 0f;
		while (counter < healthLost) {
			counter++;
			healthBar.transform.localRotation = Quaternion.Euler (0, 0, Random.Range (-2.5f, 2.5f));
			float rotationThisFrame = Random.Range (-2.5f, 2.5f);
			inverseCameraRotation -= rotationThisFrame;
			Camera.main.transform.Rotate(0, 0, rotationThisFrame);
			yield return null;
		}
		Camera.main.transform.Rotate (0, 0, inverseCameraRotation);
		healthBar.transform.localRotation = Quaternion.identity;
		yield break;
	}
}
