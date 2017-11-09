using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDriver : MonoBehaviour {

	public int numberOfPlayers;

	private int[] placement;

	// Use this for initialization
	void Start () {
		placement = new int[2];
		for (int i = 0; i < placement.Length; i++) {
			placement [i] = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int Finish(int playerNum){
		for (int i = 0; i < placement.Length; i++) {
			if (placement [i] == 0) {
				placement [i] = playerNum;
				return (i + 1);
			}
			if (placement [i] == playerNum) {
				return (i + 1);
			}
		}
		return 0;
	}
}
