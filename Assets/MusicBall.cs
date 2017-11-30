using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBall : MonoBehaviour {

	public Vector3 rotation;
	public float respawnTime;
	private bool collected = false;
	private float resTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(rotation);
		if (collected) {
			resTimer += Time.deltaTime;
			if (resTimer >= respawnTime) {
				resTimer = 0f;
				Respawn();
			}
		}
	}

	public void SetCollected(){
		collected = true;
		this.GetComponent<SphereCollider> ().enabled = false;
		this.GetComponent<MeshRenderer> ().enabled = false;
	}

	public void Respawn(){
		collected = false;
		this.GetComponent<SphereCollider>().enabled = true;
		this.GetComponent<MeshRenderer> ().enabled = true;;
	}

	public bool IsCollected(){
		return collected;
	}
}
