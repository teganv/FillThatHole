using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hole : MonoBehaviour {

	public int blockWidth = 0; //the number of 1x1 blocks wide the hole is

	public int area = 0; // the number of 1x1 blocks making up the entire hole

	public int filledArea = 0; // the number of blocks filled

	public bool hasBump = false; //Was this hole overfilled, invalidating it from perfect fill bonuses?

	private GameManager gameManager;

	void Start() {
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		UpdateGameManagerPercentFilled ();
	}

	public void MadeBump() {
		hasBump = true;
	}

	public void UpdateGameManagerPercentFilled() {
		gameManager.UpdatePercentFilled ();
	}
		
}
