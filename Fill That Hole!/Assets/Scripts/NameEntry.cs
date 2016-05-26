using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameEntry : MonoBehaviour {
	public InputField inputField;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			PlayerPrefs.SetString ("score" + PlayerPrefs.GetInt ("newScore").ToString () + "name", inputField.text);
			SceneManager.LoadScene ("End");
		}
	}
}
