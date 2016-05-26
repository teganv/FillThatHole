using UnityEngine;
using System.Collections;

public class TitleLogo : MonoBehaviour {


	public void Spin() {
		StopAllCoroutines ();
		StartCoroutine (SpinAwayFromTitle ());
	}

	private IEnumerator SpinAwayFromTitle() {
		Vector3 to = new Vector3(0, 0, 190);
		while (Vector3.Distance(transform.eulerAngles, to) > 0.1f)
		{
			transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to,  .7f * Time.deltaTime);
			yield return null;
		}
		transform.eulerAngles = to;
	}

}
