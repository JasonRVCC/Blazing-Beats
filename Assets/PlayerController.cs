using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	[Header("Movement")]
	public float acceleration;
	public float maxVelocity;
	public float deceleration;
	[Space(10)]

	public int playerNum;

	[Header("Tilting")]
	public float leanHorizontal;
	public float maxAngleLeft;
	public float maxAngleRight;
	public float leanVertical;
	public float maxAngleVertical;


	private Transform playerTransform;
	private Vector3 curVelocity;
	private Vector3 eulerRotation;
	private Vector3 curRotation;

	// Use this for initialization
	void Start () {
		playerTransform = this.transform;
		curVelocity = this.gameObject.GetComponent<Rigidbody> ().velocity;
		curRotation = playerTransform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		//Player Movement
		playerTransform.position += playerTransform.forward * acceleration * -Input.GetAxis("Vertical"+playerNum) * Time.deltaTime;
		//this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, 0, acceleration * -Input.GetAxis ("Vertical")));
		//this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, acceleration * -Input.GetAxis ("Vertical"));
		this.gameObject.transform.Rotate(new Vector3(0,leanHorizontal * Input.GetAxis("Horizontal"+playerNum),0));
		//curVelocity = this.gameObject.GetComponent<Rigidbody> ().velocity;
		//Debug.Log (curVelocity);

		//Player lean
		/*
		if (curVelocity.x > 0) {
			eulerRotation.z = Mathf.Max (- leanHorizontal, maxAngleLeft + curRotation.z);
		} else if (curVelocity.x < 0) {
			eulerRotation.z = Mathf.Min (leanHorizontal, maxAngleRight - curRotation.z);
		} else {
			if (eulerRotation.z < 0) {
				eulerRotation.z = Mathf.Min (leanHorizontal, 0 - curRotation.z);
			} else if (eulerRotation.z > 0) {
				eulerRotation.z = Mathf.Max (-leanHorizontal, 0 - curRotation.z);
			}
		}
		if (curVelocity.z > 0) {
			eulerRotation.x = Mathf.Max (- leanVertical, -maxAngleVertical + curRotation.x);
		} else if (curVelocity.z < 0) {
			eulerRotation.x = Mathf.Min (leanVertical, maxAngleVertical - curRotation.x);
		} else {
			if (eulerRotation.x < 0) {
				eulerRotation.x = Mathf.Min (leanVertical, 0 - curRotation.x);
			} else if (eulerRotation.x > 0) {
				eulerRotation.x = Mathf.Max (-leanVertical, 0 - curRotation.x);
			}
		}

		playerTransform.Rotate (eulerRotation);
		curRotation = playerTransform.rotation.eulerAngles;
		*/
	}
}
