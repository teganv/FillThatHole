using UnityEngine;
using System.Collections;

public class CameraTargetHole : MonoBehaviour {

	public Vector3 target;
	private Quaternion targetRot;

	private Coroutine follow;


	void Start() {
		Transform car = GameObject.Find ("car").transform;
		follow = StartCoroutine (Follow (car));;
	}

	public void SetTarget(Vector3 targ) {
		StopCoroutine (follow);
		target = targ;
		StartCoroutine (MoveIntoPlace ());
	}

	IEnumerator Follow(Transform car) {
		while (true) {
			transform.position = new Vector3 (car.position.x + 8, transform.position.y, transform.position.z);
			yield return null;
		}
	}


	IEnumerator MoveIntoPlace() {
		while (transform.position.x != target.x - 3.5f) {
			transform.position = Vector3.MoveTowards (transform.position, new Vector3(target.x - 3.5f, 2.32f, -10), 18 * Time.deltaTime);
			yield return null;
		}
	}
}
