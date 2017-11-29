using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompareAxis{X,Y,Z};

public class GameDriver : MonoBehaviour {

	public int numberOfPlayers;
	public int wayPoints;

	[Header("Waypoint Comparison")]
	public CompareAxis[] wayCompares;

	private int[] placement;

	// Use this for initialization
	void Start () {
		placement = new int[2];
		for (int i = 0; i < placement.Length; i++) {
			placement [i] = 0;
		}
		if (wayCompares.Length != wayPoints) {
			Debug.LogError ("WARNING: array wayCompares is not equal to the number of way points in the level.");
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//public bool AheadOfOtherPlayer

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
