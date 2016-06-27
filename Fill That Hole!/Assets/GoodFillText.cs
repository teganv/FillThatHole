using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoodFillText : MonoBehaviour {

	private float rotDir;
	private Text text;
	private Animator animator;

	// Use this for initialization
	void Start () {
		rotDir = Random.Range (-.12f, .12f);
		text = GetComponent<Text> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotDir);
	}

	void OnEnable() {
		rotDir = Random.Range (-.12f, .12f);
	}
}
