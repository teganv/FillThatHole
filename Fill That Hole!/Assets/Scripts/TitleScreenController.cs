using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {

	private bool playingChant;
	public Text controlsText;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		playingChant = false;
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetJoystickNames ().Length > 0) {
			controlsText.text = "Press 'A'\nto start\n\nPress 'B'\nfor controls";
			if (Input.GetButtonDown ("Vroom")) {
				SceneManager.LoadScene ("GamepadControls");
			}
			else if (Input.GetButtonDown ("Shift") && !playingChant) {
				PlayerPrefs.SetInt ("Holes Cleared", 0);
				PlayerPrefs.SetFloat ("Car Speed", 1.3f);
				StartCoroutine (PlayChantThenStart ());
			}
		} else {
			controlsText.text = "Press space\nto start\n\nPress shift\nfor controls";
			if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
				SceneManager.LoadScene ("KeyboardControls");
			}
			else if (Input.GetButtonDown ("Vroom") && !playingChant) {
				PlayerPrefs.SetInt ("Holes Cleared", 0);
				PlayerPrefs.SetFloat ("Car Speed", 1.3f);
				StartCoroutine (PlayChantThenStart ());
			}
		}
	}

	private IEnumerator PlayChantThenStart() {
		playingChant = true;
		source.Play ();
		while (source.isPlaying) {
			yield return null;
		}
		SceneManager.LoadScene ("Main");
	}
}
