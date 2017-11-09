using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBall : MonoBehaviour {

	public Vector3 rotation;
	private bool collected = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(rotation);
	}

	public void SetCollected(){
		collected = true;
	}

	public bool IsCollected(){
		return collected;
	}
}
