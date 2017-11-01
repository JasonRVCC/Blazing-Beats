using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour {

	public int playerNum;
	private Transform playerTransform;

	// Use this for initialization
	void Start () {
		this.gameObject.layer = LayerMask.NameToLayer( "p" + playerNum);
		playerTransform = GameObject.FindGameObjectWithTag ("Player" + playerNum).transform;
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 rot = transform.localRotation.eulerAngles;
//		rot.y = 0;
//		transform.localRotation = Quaternion.Euler (rot);
		transform.LookAt(playerTransform);
		Vector3 rot = transform.rotation.eulerAngles;
		rot.x = 0;
		rot.z = 0;
		transform.rotation = Quaternion.Euler (rot);
	}
}
