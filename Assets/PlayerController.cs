using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {



	public PlayerCamera plCamera;
	public int playerNum;

	[Header("Movement")]
	public float acceleration;
	public float maxSpeed;
	public float deceleration;
	public float maxturn;
	public float maxturndecrease;
	[Space(8)]

	[Header("Power Ups")]
	[Header("Boost")]
	public int BoostCost;
	public float BoostSpeed;
	public float BoostSlowdown;
	[Space(4)]
	[Header("Double Orb")]
	public int DoubleCost;
	public float DoubleTime;
	[Space(4)]
	[Header("Tuba")]
	public int TubaCost;
	public float TubaRotateSpeed;
	public float TubaTime;
	[Space(4)]
	[Header("Invincibility")]
	public int InvincibleCost;
	public float InvincibleTime;
	[Space(4)]
	[Header("Jazz")]
	public int JazzCost;
	public float JazzDecrease;
	public int JazzTime;
	[Space(4)]

	[Header("UI")]
	public Text collectText;
	public Text lapText;
	public Text victoryText;
	public Image powerUp1;
	public Image powerUp2;
	public Image powerUp3;
	public Text powerCost1;
	public Text powerCost2;
	public Text powerCost3;
	[Space(8)]


	[Header("Spites")]
	public Sprite BoostSprite;
	public Sprite DoubleOrbSprite;
	public Sprite TubaSprite;
	public Sprite InvincibilitySprite;
	public Sprite JazzSprite;
	[Space(8)]


	[Header("Tilting")]
	public float leanHorizontal;
	public float maxAngleLeft;
	public float maxAngleRight;
	public float leanVertical;
	public float maxAngleVertical;
	[Space(8)]

	[Header("Audio")]
	public AudioSource Step;
	public AudioSource BoostSound;
	public AudioSource GetPowerUp;
	public AudioSource PowerUpSound;
	public AudioSource LapSound;
	public float pitch;
	public float StepTime;
	[Space(8)]

	//reference to Game Driver and other player----------
	private GameDriver gameDriver;
	private GameObject otherPlayer;

	//Lap and Placement variables------------------------
	private int curLap = 1;
	private int laps = 3;
	private int place = 0;
	private int wayPointNum = 0;

	//Audio variables------------------------------------
	private bool secondStep = false;
	private float curStepTime = 0f;

	//Movement variables---------------------------------
	private float maxMag;
	private Transform playerTransform;
	private Vector3 rotateVector;
	private Vector3 eulerRotation;
	private Vector3 curRotation;
	private int collectCount = 0;
	private float curMagnitude = 0f;

	//Power up variables-------------------------------
	//Boost--------------------------------------------
	private float boostTimer = 0f;
	private bool BoostButtonDown = false;
	//PowerUp 3--------------------------------------
	private bool PowerUp2ButtonDown = false;
	//Double Orb--------------------------------------
	private float doubleOrbTimer = 0f;
	private bool isDoubleOn = false;
	//Invincibility------------------------------------
	private float invincibleTimer= 0f;
	private bool isInvincible = false;
	//PowerUp 3--------------------------------------
	private bool PowerUp3ButtonDown = false;
	//Tuba------------------------------------------
	private bool isTubaHit = false;
	private float tubaTimer = 0f;
	//Jazz--------------------------------------------
	private bool isJazzed = false;
	private float jazzTimer = 0f;



	// Use this for initialization
	void Start (){
		maxMag = maxSpeed;
		place = playerNum;
		gameDriver = GameObject.FindGameObjectWithTag ("GameDriver").GetComponent<GameDriver> ();
		if (playerNum == 1) {
			otherPlayer = GameObject.FindGameObjectWithTag ("Player2");
		} else {
			otherPlayer = GameObject.FindGameObjectWithTag ("Player1");
		}
		playerTransform = this.transform;
		victoryText.text = "";
		curRotation = playerTransform.rotation.eulerAngles;
		plCamera.SetViewPort ((playerNum - 1) * 0.5f, 0f, 0.5f, 1.0f);
		powerCost1.text = BoostCost.ToString ();

		//Temp Code for visual effect
		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
		//
	}

	public int CurrentLap{
		get{return curLap;}
	}

	public int WayPointNumber{
		get{return wayPointNum;}
	}

	
	// Update is called once per frame
	void Update () {
		SetPlace ();
		UpdatePowerUI ();

		//check if inputs are released
		if (Input.GetAxis ("Boost" + playerNum) == 0) {
			BoostButtonDown = false;
		}
		if (Input.GetAxis ("Power2" + playerNum) == 0) {
			PowerUp2ButtonDown = false;
		}
		if (Input.GetAxis ("Power3" + playerNum) == 0) {
			PowerUp3ButtonDown = false;
		}
			
		//Check boost input
		if(Input.GetAxis("Boost" + playerNum) != 0 && collectCount >= BoostCost && !BoostButtonDown){
			Debug.Log ("BOOOST");
			BoostButtonDown = true;
			Boost ();
		}
		//Check Power 2 input
		if(Input.GetAxis("Power2" + playerNum) != 0 && !PowerUp2ButtonDown){
			if (place == 1 && collectCount >= InvincibleCost) {
				Debug.Log ("Invincible");
				PowerUp2ButtonDown = true;
				TurnInvincible ();
			} else if(place !=1 && collectCount >= DoubleCost) {
				Debug.Log ("Double Orb");
				PowerUp2ButtonDown = true;
				DoubleOrb();
			}
		}
		//Check Power 3 input
		if(Input.GetAxis("Power3" + playerNum) != 0 && !PowerUp3ButtonDown){
			if (place == 1 && collectCount >= JazzCost) {
				Debug.Log ("JAZZY");
				PowerUp3ButtonDown = true;
				SetJazzTrap ();
			} else if(place != 1 && collectCount >= TubaCost) {
				Debug.Log ("GET TUBA'D");
				collectCount -= TubaCost;
				PowerUp3ButtonDown = true;
				gameDriver.SpawnTuba (otherPlayer, tubaTimer + 0.5f);
				otherPlayer.GetComponent<PlayerController> ().TubaHit (TubaRotateSpeed, TubaTime);
			}
		}

		//Count down timers
		if (boostTimer != 0) {
			boostTimer = Mathf.Max(0,boostTimer - Time.deltaTime);
			if (boostTimer == 0) {
				maxSpeed = maxMag;
			} else {
				maxSpeed = maxMag + (BoostSpeed * (boostTimer / BoostSlowdown));
			}
		}
		if (isDoubleOn) {
			doubleOrbTimer = Mathf.Max(0,doubleOrbTimer - Time.deltaTime);
			if (doubleOrbTimer == 0) {
				isDoubleOn = false;
				this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			}
		}
		if (isInvincible) {
			invincibleTimer = Mathf.Max(0,invincibleTimer - Time.deltaTime);
			if (invincibleTimer == 0) {
				isInvincible = false;
				this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			}
		}
		if (isJazzed) {
			jazzTimer = Mathf.Max(0,jazzTimer - Time.deltaTime);
			if (jazzTimer == 0) {
				isJazzed = false;
				maxMag += JazzDecrease;
				maxSpeed += JazzDecrease;
				this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
			}
		}

		//Tuba'd
		if (isTubaHit) {
			tubaTimer = Mathf.Max(0,tubaTimer - Time.deltaTime);
			if (tubaTimer == 0) {
				isTubaHit = false;
			} else {
				transform.Rotate (0, TubaRotateSpeed * Time.deltaTime, 0);
			}
		}

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
			//Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);
		} else {
			//AS.PlayOneShot (Step);
		}
		transform.Translate(transform.right * Input.GetAxis ("LeftX" + playerNum) * Time.deltaTime * 2, Space.World);
		//this.GetComponent<Rigidbody> ().AddForce (transform.right * 100f * Input.GetAxis ("LeftX" + playerNum), ForceMode.Acceleration);
		//Debug.Log (this.GetComponent<Rigidbody> ().velocity.magnitude);

		curMagnitude = this.GetComponent<Rigidbody> ().velocity.magnitude;
		if (curMagnitude > 0.01) {
			curStepTime += Time.deltaTime;
			if (curStepTime >= StepTime) {
				Step.Play ();
				Step.pitch = pitch + curMagnitude / maxSpeed;
				if (secondStep) {
					Step.pitch += 0.25f;
					secondStep = false;
				} else {
					Step.pitch -= 0.25f;
					secondStep = true;
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

	public void SetPlace(){
		if (curLap > otherPlayer.GetComponent<PlayerController> ().CurrentLap) {
			place = 1;
		} else if (curLap < otherPlayer.GetComponent<PlayerController> ().CurrentLap) {
			place = 2;
		} else if (wayPointNum > otherPlayer.GetComponent<PlayerController> ().WayPointNumber) {
			place = 1;
		} else if (wayPointNum < otherPlayer.GetComponent<PlayerController> ().WayPointNumber) {
			place = 2;
		} else if(gameDriver.AheadOfOtherPlayer(wayPointNum,this.transform.position,otherPlayer.transform.position)){
			place = 1;
		} else {
			place = 2;
		}

	}

	public void UpdatePowerUI(){
		//Set boost UI
		if (collectCount >= BoostCost) {
			powerUp1.color = new Color (255f, 255f, 255f, 1f);
		} else {
			powerUp1.color = new Color (255f, 255f, 255f,0.5f);;
		}
		//Set Power Up 2 and 3 UI
		if (place == 1) {
			powerUp2.sprite = InvincibilitySprite;
			powerCost2.text = InvincibleCost.ToString ();
			if (collectCount >= InvincibleCost && !isInvincible) {
				powerUp2.color = new Color (255f, 255f, 255f, 1f);
			} else {
				powerUp2.color = new Color (255f, 255f, 255f,0.5f);;
			}
			powerUp3.sprite = JazzSprite;
			powerCost3.text = JazzCost.ToString ();
			if (collectCount >= JazzCost) {
				powerUp3.color = new Color (255f, 255f, 255f, 1f);
			} else {
				powerUp3.color = new Color (255f, 255f, 255f,0.5f);;
			}
		} else {
			powerUp2.sprite = DoubleOrbSprite;
			powerCost2.text = DoubleCost.ToString ();
			if (collectCount >= DoubleCost && !isDoubleOn) {
				powerUp2.color = new Color (255f, 255f, 255f, 1f);
			} else {
				powerUp2.color = new Color (255f, 255f, 255f,0.5f);;
			}
			powerUp3.sprite = TubaSprite;
			powerCost3.text = TubaCost.ToString ();
			if (collectCount >= TubaCost) {
				powerUp3.color = new Color (255f, 255f, 255f, 1f);
			} else {
				powerUp3.color = new Color (255f, 255f, 255f,0.5f);;
			}
		}
	}

	public void Boost(){
		collectCount -= BoostCost;
		maxSpeed += BoostSpeed;
		if (this.GetComponent<Rigidbody> ().velocity.magnitude < 0.01 && this.GetComponent<Rigidbody> ().velocity.magnitude > -0.01) {
			Debug.Log ("ADDDDDD");
			this.GetComponent<Rigidbody> ().AddForce (transform.forward * 2000, ForceMode.Acceleration);
			BoostSound.Play ();
		}
		this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized * maxSpeed;
		boostTimer = BoostSlowdown;
	}

	public void DoubleOrb(){
		collectCount -= DoubleCost;
		isDoubleOn = true;
		PowerUpSound.Play ();
		doubleOrbTimer = DoubleTime;
		//Temp Code for visual effect
		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		//
	}

	public void TurnInvincible(){
		collectCount -= InvincibleCost;
		isInvincible = true;
		PowerUpSound.Play ();
		invincibleTimer = InvincibleTime;
		//Temp Code for visual effect
		this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		//
	}

	//Get Tuba'd
	public void TubaHit(float tubaForce, float tubaTime){
		if (!isInvincible) {
			isTubaHit = true;
			tubaTimer = tubaTime;
		}
	}

	//Spawn Jazz Trap
	public void SetJazzTrap(){
		collectCount -= JazzCost;
		gameDriver.SpawnJazzTrap (this.gameObject, playerNum, JazzTime);
	}


	void OnTriggerEnter(Collider other){
		Debug.Log ("hit");
		string tag = other.gameObject.tag;
		if (tag == "Collectable") {
			if (!other.gameObject.GetComponent<MusicBall> ().IsCollected ()) {
				other.gameObject.GetComponent<MusicBall> ().SetCollected ();
				GetPowerUp.Play ();
				if (isDoubleOn) {
					collectCount += 2;
				} else {
					collectCount += 1;
				}
			}
		}/*
		if(tag == "Check1"){
			if(passedCheckPoint){
				passedCheckPoint = false;
			}
			else if(!passedCheckPoint){
				passedCheckPoint = true;
			}
		}*/
		if (tag == "Waypoint") {
			int prevWay = wayPointNum;
			wayPointNum = other.gameObject.GetComponent<Waypoint> ().wayNumber;
			if (prevWay == 8 && wayPointNum == 0) {
				if (curLap < laps) {
					curLap += 1;
					LapSound.Play ();
				} else if (curLap == laps) {
					int finishPlace = gameDriver.Finish (playerNum);
					if (finishPlace == 1) {
						victoryText.text = "You Win!";
					} else {
						victoryText.text = "You Lose!";
					}
				}
			}
		}
		if (tag == "Jazz") {
			int jazzNum = other.gameObject.GetComponent<JazzControl> ().playerNum;
			if (jazzNum != -1 && jazzNum != playerNum) {
				if (!isInvincible) {
					isJazzed = true;
					gameDriver.JazzSound.Play ();
					maxMag -= JazzDecrease;
					maxSpeed -= JazzDecrease;
					this.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", new Color (186, 85, 211));
					jazzTimer = JazzTime;
				}
				GameObject.Destroy (other);
			}
		}
		/*
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
		} */
	}
}
