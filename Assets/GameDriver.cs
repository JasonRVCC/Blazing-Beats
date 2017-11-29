using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompareAxis{X,NegX,Y,NegY,Z,NegZ};

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

	public bool AheadOfOtherPlayer(int compareMethod, Vector3 fp, Vector3 sp){
		switch (wayCompares[compareMethod]) {
			case CompareAxis.X:
				if (fp.x > sp.x) {
					return true;
				} else {
					return false;
				}
			case CompareAxis.NegX:
				if (fp.x < sp.x) {
					return true;
				} else {
					return false;
				}
			case CompareAxis.Y:
				if (fp.y > sp.y) {
					return true;
				} else {
					return false;
				}
			case CompareAxis.NegY:
				if (fp.y < sp.y) {
					return true;
				} else {
					return false;
				}
			case CompareAxis.Z:
				if (fp.z > sp.z) {
					return true;
				} else {
					return false;
				}
			case CompareAxis.NegZ:
				if (fp.z < sp.z) {
					return true;
				} else {
					return false;
				}
		}
		Debug.LogError ("Waypoint Comparison " + compareMethod + " not set.");
		return false;
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
