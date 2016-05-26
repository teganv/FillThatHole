using UnityEngine;
using System.Collections;

public class DestroyOffScreen : MonoBehaviour {

	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
