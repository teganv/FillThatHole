using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private float speed;
	private float scale;

	// Use this for initialization
	void Start () {
		speed = Random.Range (.3f, 2.2f);
		scale = Random.Range (.9f, 2f);
		transform.localScale = transform.localScale * scale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position -= new Vector3 (speed, 0, 0) * Time.deltaTime;
	}

	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
