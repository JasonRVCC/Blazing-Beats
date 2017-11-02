using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	[Header("Movement")]
	public float acceleration;
	public float maxSpeed;
	public float deceleration;
	[Space(10)]

	public int playerNum;
	public Text collectText;

	[Header("Tilting")]
	public float leanHorizontal;
	public float maxAngleLeft;
	public float maxAngleRight;
	public float leanVertical;
	public float maxAngleVertical;


	private Transform playerTransform;
	private Vector3 eulerRotation;
	private Vector3 curRotation;
	private int collectCount = 0;

	// Use this for initialization
	void Start (){
		playerTransform = this.transform;
		curRotation = playerTransform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		collectText.text = "Orbs" + collectCount;
		//Player Movement
		//Vector3 move = playerTransform.rotation * Vector3.forward;
		/*
		float axisValue = Input.GetAxis ("Vertical" + playerNum);
		if (axisValue != 0) {
			if (axisValue < 0) {
				curSpeed = Mathf.Max (-maxSpeed, curSpeed - (acceleration * Time.deltaTime));
			} else {
				curSpeed = Mathf.Min (maxSpeed, curSpeed + (acceleration * Time.deltaTime));
			}
		} else {
			if (curSpeed > 0) {
				curSpeed = Mathf.Max (0f, curSpeed - (deceleration * Time.deltaTime));
			} else if (curSpeed < 0) {
				curSpeed = Mathf.Min(0f, curSpeed + (deceleration * Time.deltaTime));
			}
		}

		//Brake
		if (Input.GetAxis ("Brake" + playerNum) != 0) {
			Debug.Log ("BRAKE");
			if (curSpeed > 0) {
				curSpeed = Mathf.Max (0f, curSpeed - (deceleration * Time.deltaTime * 2));
			} else if (curSpeed < 0) {
				curSpeed = Mathf.Min(0f, curSpeed + (deceleration * Time.deltaTime * 2));
			}
		}*/

		//Debug.Log (curSpeed);
		this.GetComponent<Rigidbody> ().AddForce (transform.forward * acceleration * Input.GetAxis("Vertical"+playerNum), ForceMode.Acceleration);
		if (this.GetComponent<Rigidbody> ().velocity.magnitude > maxSpeed) {
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized * maxSpeed;
			Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);
		}
		//this.GetComponent<Rigidbody> ().velocity = move * curSpeed;
		//playerTransform.position += playerTransform.forward * curSpeed/4;
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

	void OnTriggerEnter(Collider other){
		Debug.Log ("hit");
		if (other.gameObject.tag == "Collectable") {
			collectCount += 1;
			GameObject.Destroy (other.gameObject);
		}
	}
}
