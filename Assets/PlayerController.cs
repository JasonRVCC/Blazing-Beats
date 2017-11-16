using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public AudioSource AS;
	public AudioClip GetPowerup;
	public AudioClip Step;
	public AudioClip Fast;

	[Header("Movement")]
	public float acceleration;
	public float maxSpeed;
	public float deceleration;
	[Space(10)]

	public int playerNum;
	public Text collectText;
	public Text lapText;
	public Text victoryText;

	public float maxturn;
	public float maxturndecrease;

	[Header("Tilting")]
	public float leanHorizontal;
	public float maxAngleLeft;
	public float maxAngleRight;
	public float leanVertical;
	public float maxAngleVertical;

	public float StepTime;


	private int curLap = 1;
	private int laps = 3;
	private int place = 0;

	private GameDriver gameDriver;

	private bool passedCheckPoint = false;

	private Transform playerTransform;
	private Vector3 rotateVector;
	private Vector3 eulerRotation;
	private Vector3 curRotation;
	private int collectCount = 0;
	private float curStepTime = 0f;
	private float curMagnitude = 0f;

	// Use this for initialization
	void Start (){
		gameDriver = GameObject.FindGameObjectWithTag ("GameDriver").GetComponent<GameDriver> ();
		playerTransform = this.transform;
		victoryText.text = "";
		curRotation = playerTransform.rotation.eulerAngles;
	}

	
	// Update is called once per frame
	void Update () {
		collectText.text = "Orbs" + collectCount;
		lapText.text = "Lap: " + curLap + "/" + laps;

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
		this.gameObject.transform.Rotate(new Vector3(0,(maxturn - (maxturndecrease * (this.GetComponent<Rigidbody>().velocity.magnitude/maxSpeed))) * Input.GetAxis("Horizontal"+playerNum) * Time.deltaTime,0));
		if (transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity).z >= 0) {
			rotateVector = transform.forward;
		} else {
			rotateVector = transform.forward * -1;
		}
		this.GetComponent<Rigidbody>().velocity = Vector3.RotateTowards (this.GetComponent<Rigidbody> ().velocity, rotateVector, 360.0f, 0.0f);
		//this.GetComponent<Rigidbody> ().AddTorque (transform.up * leanHorizontal * Input.GetAxis("Horizontal"+playerNum));
		if (Input.GetAxis ("Foward" + playerNum) != 0) {
			this.GetComponent<Rigidbody> ().AddForce (transform.forward * acceleration * Input.GetAxis ("Foward" + playerNum) , ForceMode.Acceleration);
		} else if (this.GetComponent<Rigidbody> ().velocity.magnitude != 0) {
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized * Mathf.Max (0, this.GetComponent<Rigidbody> ().velocity.magnitude - (deceleration * Time.deltaTime));
			//this.GetComponent<Rigidbody> ().AddForce (rotateVector * -1 * deceleration, ForceMode.Acceleration);
			//if(this.GetComponent<Rigidbody> ().velocity.magnitude
		}
		if (this.GetComponent<Rigidbody> ().velocity.magnitude > maxSpeed) {
			//AS.PlayOneShot (Fast);
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized * maxSpeed;
			Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);
		} else {
			//AS.PlayOneShot (Step);
		}
		transform.Translate(transform.right * Input.GetAxis ("LeftX" + playerNum) * Time.deltaTime * 2, Space.World);
		//this.GetComponent<Rigidbody> ().AddForce (transform.right * 100f * Input.GetAxis ("LeftX" + playerNum), ForceMode.Acceleration);
		Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);

		curMagnitude = this.GetComponent<Rigidbody> ().velocity.magnitude;
		if (curMagnitude > 0.01) {
			curStepTime += Time.deltaTime;
			if (curStepTime >= StepTime) {
				if (curMagnitude < maxSpeed) {
					AS.PlayOneShot (Step);
				} else {
					AS.PlayOneShot (Fast);
				}
				curStepTime = 0f;
			}
		} else  {
			curStepTime = 0f;
		}


		//this.GetComponent<Rigidbody> ().velocity = move * curSpeed;
		//playerTransform.position += playerTransform.forward * curSpeed/4;
		//this.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, 0, acceleration * -Input.GetAxis ("Vertical")));
		//this.gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, acceleration * -Input.GetAxis ("Vertical"));
		//this.gameObject.transform.Rotate(new Vector3(0,leanHorizontal * Input.GetAxis("Horizontal"+playerNum),0));
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
		string tag = other.gameObject.tag;
		if (tag == "Collectable") {
			if (!other.gameObject.GetComponent<MusicBall> ().IsCollected ()) {
				AS.PlayOneShot (GetPowerup);
				other.gameObject.GetComponent<MusicBall> ().SetCollected ();
				GameObject.Destroy (other.gameObject);
				collectCount += 1;
			}
		}
		if(tag == "Check1"){
			if(passedCheckPoint){
				passedCheckPoint = false;
			}
			else if(!passedCheckPoint){
				passedCheckPoint = true;
			}
		}
		if (tag == "FinishLine") {
			if (passedCheckPoint) {
				if (curLap < laps) {
					curLap += 1;
				} else if (curLap == laps) {
					place = gameDriver.Finish (playerNum);
					if (place%10 == 1) {
						victoryText.text = place + "st";
					}
					else if (place % 10 == 2) {
						victoryText.text = place + "nd";
					} else if(place%10 == 3){
						victoryText.text = place + "rd";
					}else{
						victoryText.text = place + "th";
					}
				}
				passedCheckPoint = false;
			}
		}
	}
}
