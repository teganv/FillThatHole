using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScreenController : MonoBehaviour {

	public Text[] scoreTexts;

	public GameObject screenExitWipeTransition;

	// Use this for initialization
	void Start () {
		/*PlayerPrefs.DeleteAll ();
		for(int i = 0; i < 10; i ++) {
			PlayerPrefs.SetString ("score" + (i + 1).ToString () + "name", "Aspiring Hole Filler");
			PlayerPrefs.SetInt ("score" + (i + 1).ToString (), 10 - i);
		}*/
		int holes = PlayerPrefs.GetInt ("Holes Cleared");
		GetComponent<Text> ().text = "You cleared " + holes + "\nholes before\nascending";
		for(int i = 0; i < 10; i ++) {
			scoreTexts [i].text = (i + 1).ToString () + ". " + PlayerPrefs.GetString ("score" + (i + 1).ToString () + "name", "nope") + " - " + PlayerPrefs.GetInt ("score" + (i + 1).ToString (), 0) + " Holes";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Drop") || Input.GetButtonDown ("Vroom") || Input.GetButtonDown ("Shift"))
			StartCoroutine (TransitionToMainScene ());
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetInt ("Holes Cleared", 0);
	}

	private IEnumerator TransitionToMainScene() {
		screenExitWipeTransition.SetActive (true);
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("Main");
	}
}
