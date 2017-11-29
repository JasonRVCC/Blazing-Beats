using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
	public int wayNumber;

	void Awake(){
		this.GetComponent<MeshRenderer> ().enabled = false;
	}
}
