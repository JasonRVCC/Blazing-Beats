using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubaControl : MonoBehaviour {

	public float deathTimer;
	public bool started = false;
	
	// Update is called once per frame
	void Update () {
		if (started) {
			deathTimer -= Time.deltaTime;
			if (deathTimer <= 0) {
				GameObject.Destroy (this.gameObject);
			}
		}
	}

	public void StartDeath(float time){
		deathTimer = time;
		started = true;
	}
}
