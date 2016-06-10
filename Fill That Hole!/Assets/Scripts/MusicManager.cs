using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	static bool AudioBegin = false;
	//static bool gameAlreadyStarted = false;
	GameObject otherSound;

	private AudioSource source; //the source that plays the chant


	void Awake()
	{
		otherSound = GameObject.Find("MusicManager");

		if (otherSound == this.gameObject)
		{
			if (!AudioBegin)
			{
				DontDestroyOnLoad(this.gameObject);
				AudioBegin = true;
			}
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
		
	void Start() {
		source = GetComponents<AudioSource> ()[1];
	}

	public void playChant() {
		source.Play ();
	}

}
