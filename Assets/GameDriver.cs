﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CompareAxis{X,NegX,Y,NegY,Z,NegZ};

public class GameDriver : MonoBehaviour {

	public int numberOfPlayers;
	public int wayPoints;

	public AudioSource BGM;
	public AudioSource Ambeince;
	public AudioSource TubaSound;
	public AudioSource JazzSound;

	public float BGMStartTime = 0f;

	[Header("Power Up Prefabs")]
	public GameObject TubaPrefab;
	public float TubaSpawnDistance;
	public GameObject JazzPrefab;
	[Space(8)]

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
		BGM.time = BGMStartTime;
		BGM.Play ();
		Ambeince.Play ();


	}
	
	// Update is called once per frame
	void Update () {
		if (TubaSound.isPlaying && TubaSound.time >= 2) {
			TubaSound.Stop ();
		}
	}

	public void SpawnTuba(GameObject player, float tubaTime){
		Vector3 spawnPos = player.transform.position + player.transform.forward * TubaSpawnDistance;
		spawnPos.y += 1.6f;
		GameObject tuba = (GameObject)Instantiate (TubaPrefab, spawnPos, Quaternion.Inverse( player.transform.rotation) );
		tuba.GetComponent<TubaControl>().StartDeath (tubaTime);
		TubaSound.Play ();
	}

	public void SpawnJazzTrap(GameObject player, int playerNum, float jazzTime){
		GameObject jazz = (GameObject)Instantiate (JazzPrefab, player.transform.position, player.transform.rotation);
		jazz.GetComponent<JazzControl> ().playerNum = playerNum;
		jazz.GetComponent<JazzControl> ().jazzTime = jazzTime;
		if (player.GetComponent<PlayerController> ().playerNum == 1) {
			jazz.GetComponent<Renderer> ().material.SetColor ("_Color", new Color (255, 0, 0, 0.47f));
		} else {
			jazz.GetComponent<Renderer> ().material.SetColor ("_Color", new Color (0, 255, 0, 0.47f));
		}
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

	public void Finish(int playerNum){
		SceneManager.LoadScene ("Victory" + playerNum);
	}
}
