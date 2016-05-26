using UnityEngine;
using System.Collections;

public class FillSquadStartAnimation : MonoBehaviour {

	public GameObject[] fillSquad;

	// Use this for initialization
	void Start () {
		FlyAnimation ();
	}
	
	private IEnumerator DeactivateAfterAnimation() {
		yield return new WaitForSeconds (5);
		gameObject.SetActive (false);
	}

	public void FlyAnimation() {
		for (int i = 0; i < fillSquad.Length; i++) {
			fillSquad [i].transform.position = new Vector3 (Random.Range (Camera.main.transform.position.x - 7f, Camera.main.transform.position.x + 19f), Random.Range (-15f, -20f), 150);
			fillSquad [i].GetComponent<Rigidbody2D> ().velocity = new Vector2 (15, 15);
			StartCoroutine (DeactivateAfterAnimation ());

		}
	}
}
