using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingScreenController : MonoBehaviour {

	public Text[] scoreTexts;

	// Use this for initialization
	void Start () {
		int holes = PlayerPrefs.GetInt ("Holes Cleared");
		GetComponent<Text> ().text = "You cleared " + holes + "\nholes before\nascending";
		for(int i = 0; i < 10; i ++) {
			scoreTexts [i].text = (i + 1).ToString () + ". " + PlayerPrefs.GetString ("score" + (i + 1).ToString () + "name") + " - " + PlayerPrefs.GetInt ("score" + (i + 1).ToString ()) + " Holes";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Drop") || Input.GetButtonDown("Vroom") || Input.GetButtonDown("Shift"))
			SceneManager.LoadScene ("Main");
	}

	void OnApplicationQuit() {
		PlayerPrefs.SetInt ("Holes Cleared", 0);
		PlayerPrefs.SetFloat ("Car Speed", 1.5f);
	}
}
