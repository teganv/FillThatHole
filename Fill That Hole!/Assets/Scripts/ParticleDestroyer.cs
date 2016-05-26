using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (stopEmitting());
		StartCoroutine (destroyAfterEmitting ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator stopEmitting() {
		yield return new WaitForSeconds (.4f);
		GetComponent<ParticleSystem> ().enableEmission = false;
	}

	IEnumerator destroyAfterEmitting() {
		yield return new WaitForSeconds (6);
		Destroy (gameObject);
	}
}
