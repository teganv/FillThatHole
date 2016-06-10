using UnityEngine;
using System.Collections;

public class SlideFromBlackAnimationDestroyer : MonoBehaviour {

	RectTransform rect;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rect.anchoredPosition.x > 1500) {
			Destroy (gameObject);
		}


	}
}
