using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameEntry : MonoBehaviour {
	public InputField inputField;

	public GameObject screenExitWipeTransition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			PlayerPrefs.SetString ("score" + PlayerPrefs.GetInt ("newScore").ToString () + "name", inputField.text);
			StartCoroutine (TransitionToEndScene());
		}
	}

	private IEnumerator TransitionToEndScene() {
		screenExitWipeTransition.SetActive (true);
		yield return new WaitForSeconds (2f);
		SceneManager.LoadScene ("End");
	}
}
