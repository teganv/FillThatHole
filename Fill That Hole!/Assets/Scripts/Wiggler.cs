using UnityEngine;
using System.Collections;

public class Wiggler : MonoBehaviour {
	private float startScale, minScale, maxScale, startRotationX, minRotationX, maxRotationX, startRotationY, minRotationY, maxRotationY, startRotationZ, minRotationZ, maxRotationZ;
	private Quaternion startRotation;

	// Use this for initialization
	void Start () {
		startScale = transform.localScale.x;
		minScale = startScale - .05f;
		maxScale = startScale + .05f;

		startRotation = transform.rotation;

		startRotationX = startRotation.eulerAngles.x;
		minRotationX = startRotationX - 3f;
		maxRotationX = startRotationX + 3f;

		startRotationY = startRotation.eulerAngles.y;
		minRotationY = startRotationY - 3f;
		maxRotationY = startRotationY + 3f;

		startRotationZ = startRotation.eulerAngles.z;
		minRotationZ = startRotationZ - 3f;
		maxRotationZ = startRotationZ + 3f;

		StartCoroutine (wiggle());
	}
	
	// Update is called once per frame
	IEnumerator wiggle () {
		transform.localScale = new Vector3 (Random.Range (minScale, maxScale), Random.Range (minScale, maxScale), transform.localScale.z);
		//var rot = transform.rotation;
		//transform.rotation = Quaternion.Euler (new Vector3 (Random.Range (minRotationX, maxRotationX), Random.Range (minRotationY, maxRotationY), Random.Range (minRotationZ, maxRotationZ)));
		//transform.localRotation = Quaternion.Euler (new Vector3 (Random.Range (transform.localRotation.eulerAngles.x - 3f, transform.localRotation.eulerAngles.x + 3f), Random.Range (transform.localRotation.eulerAngles.y - 3f, transform.localRotation.eulerAngles.y + 3f), Random.Range (transform.localRotation.eulerAngles.z - 3f, transform.localRotation.eulerAngles.z + 3f)));
		yield return new WaitForSeconds (.2f);
		StartCoroutine (wiggle ());
	}

	public void UpdateRotationRanges(Vector3 rotation) {
		/*startRotation *= Quaternion.Euler(rotation);

		startRotationX = startRotation.eulerAngles.x;
		minRotationX = startRotationX - 3f;
		maxRotationX = startRotationX + 3f;

		startRotationY = startRotation.eulerAngles.y;
		minRotationY = startRotationY - 3f;
		maxRotationY = startRotationY + 3f;

		startRotationZ = startRotation.eulerAngles.z;
		minRotationZ = startRotationZ - 3f;
		maxRotationZ = startRotationZ + 3f;*/
	}
}
