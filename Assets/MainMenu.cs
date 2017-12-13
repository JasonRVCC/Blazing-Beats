using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState{Start,Main,Instructions,StageSelect,PlayerConfirm}

public class MainMenu : MonoBehaviour {
	[Header("General")]
	public float axisBuffer = 0.2f;
	public Text BackText;
	public Image CoverScreen;
	[Space(8)]

	[Header("Start")]
	public Text StartText;
	public float BlinkOutTime;
	public float BlinkInTime;
	[Space(8)]

	[Header("Main")]
	public GameObject Main;
	public Button InstructButton;
	public Button StartGameButton;
	public Button ExitButton;
	[Space(8)]

	[Header("Instructions")]
	[Space(8)]

	[Header("StageSelect")]
	public GameObject StageSelect;
	public Button CityButton;
	public Button DesertButton;
	[Space(8)]

	[Header("PlayerConfirm")]
	public GameObject PlayerConfirm;
	public Image p1Confirm;
	public Text p1Text;
	public Image p2Confirm;
	public Text p2Text;
	public Text ReadyText;
	[Space(8)]


	private MenuState state;

	private int selectedButton = 0;
	private float bufferTimer = 0f;

	//Start
	private float blinkTimer; 

	//Main
	private Button[] mainButtons;

	//Stage Select
	private Button[] stageButtons;
	private string stageSelected = "";

	//Player Confirm
	private bool p1IsIn = false;
	private bool p2IsIn = false;


	// Use this for initialization
	void Start () {
		state = MenuState.Start;
		BackText.gameObject.SetActive (false);
		CoverScreen.gameObject.SetActive(false);
		mainButtons = new Button []{InstructButton, StartGameButton, ExitButton};
		stageButtons = new Button[]{ CityButton, DesertButton };
		ReadyText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case MenuState.Start:
			blinkTimer += Time.deltaTime;
			if (StartText.color.a == 1.0f) {
				if (blinkTimer >= BlinkOutTime) {
					StartText.color = new Color (255f, 255f, 255f, 0f);
					blinkTimer = 0;
				}
			} else {
				if (blinkTimer >= BlinkInTime) {
					StartText.color = new Color (255f, 255f, 255f, 1f);
					blinkTimer = 0;
				}
			}

			if (Input.GetKey (KeyCode.Return) || Input.GetButton("Start1") || Input.GetButton("Start2")) {
				StartText.gameObject.SetActive (false);
				state = MenuState.Main;
				Main.gameObject.SetActive (true);
				SetSelectedButton (0);
			}
			break;

		case MenuState.Main:
			if (bufferTimer != 0f) {
				bufferTimer -= Time.deltaTime;
				if (bufferTimer < 0) {
					bufferTimer = 0;
				}
			} else {
				if (Input.GetAxis ("LeftY1") < 0 || Input.GetAxis ("LeftY2") < 0) {
					selectedButton += 1;
					if (selectedButton >= mainButtons.Length) {
						selectedButton = 0;
					}
					SetSelectedButton (selectedButton);
					bufferTimer = axisBuffer;
				} else if (Input.GetAxis ("LeftY1") >= 0.01 || Input.GetAxis ("LeftY2") >= 0.01) {
					selectedButton -= 1;
					if (selectedButton <= -1) {
						selectedButton = mainButtons.Length - 1;
					}
					SetSelectedButton (selectedButton);
					bufferTimer = axisBuffer;
				}
			}

			if (Input.GetButtonDown  ("Boost1") || Input.GetButtonDown  ("Boost2") || Input.GetKeyDown  (KeyCode.Return) || Input.GetButtonDown ("Start1") || Input.GetButtonDown ("Start2")) {
				mainButtons [selectedButton].gameObject.GetComponent<Image> ().color = mainButtons [selectedButton].colors.pressedColor;
				mainButtons [selectedButton].onClick.Invoke ();
			}
			break;

		case MenuState.StageSelect:
			if (Input.GetButton ("Power21") || Input.GetButton ("Power22")) {
				state = MenuState.Main;
				StageSelect.SetActive (false);
				stageButtons[selectedButton].gameObject.GetComponent<Image> ().color =  stageButtons[selectedButton].colors.normalColor;
				CoverScreen.gameObject.SetActive (false);
				BackText.gameObject.SetActive (false);
				selectedButton = 0;
				Main.SetActive (true);
				SetSelectedButton (0);
			}

			if (bufferTimer != 0f) {
				bufferTimer -= Time.deltaTime;
				if (bufferTimer < 0) {
					bufferTimer = 0;
				}
			} else {
				if (Input.GetAxis ("LeftX1") < 0 || Input.GetAxis ("LeftX2") < 0) {
					selectedButton -= 1;
					if (selectedButton < 0) {
						selectedButton = mainButtons.Length - 1;
					}
					SetSelectedButton (selectedButton);
					bufferTimer = axisBuffer;
				} else if (Input.GetAxis ("LeftX1") >= 0.01 || Input.GetAxis ("LeftX2") >= 0.01) {
					selectedButton += 1;
					if (selectedButton >= mainButtons.Length) {
						selectedButton = 0;
					}
					SetSelectedButton (selectedButton);
					bufferTimer = axisBuffer;
				}
			}

			if (Input.GetButtonDown ("Boost1") || Input.GetButtonDown  ("Boost2") || Input.GetKeyDown  (KeyCode.Return) || Input.GetButtonDown ("Start1") || Input.GetButtonDown ("Start2")) {
				stageButtons [selectedButton].gameObject.GetComponent<Image> ().color = stageButtons [selectedButton].colors.pressedColor;
				stageButtons [selectedButton].onClick.Invoke ();
			}
			break;

		case MenuState.PlayerConfirm:
			if (Input.GetButton ("Power21") || Input.GetButton ("Power22")) {
				state = MenuState.StageSelect;
				PlayerConfirm.SetActive (false);
				p1IsIn = false;
				p2IsIn = false;
				p1Text.text = "Player 1 Press Start";
				p2Text.text = "Player 2 Press Start";
				p1Confirm.color = new Color (p1Confirm.color.r, p1Confirm.color.g, p1Confirm.color.b, 0.6f);
				p2Confirm.color = new Color (p2Confirm.color.r, p2Confirm.color.g, p2Confirm.color.b, 0.6f);
				ReadyText.text = "";
				selectedButton = 0;
				stageSelected = "";
				StageSelect.SetActive (true);
				SetSelectedButton (0);
			}

			if (Input.GetButton ("Start1")) {
				if (p2IsIn && p1IsIn) {
					//Goto scene
				} else {
					p1Confirm.color = new Color (p1Confirm.color.r, p1Confirm.color.g, p1Confirm.color.b, 0.9f);
					p1Text.text = "Ready!";
					p1IsIn = true;
					if (p2IsIn) {
						ReadyText.text = "Press Start";
					}
				}
			}

			if (Input.GetButton ("Start2")) {
				if (p2IsIn && p1IsIn) {
					//Goto scene
					Debug.Log("START GAME");
				} else {
					p2Confirm.color = new Color (p2Confirm.color.r, p2Confirm.color.g, p2Confirm.color.b, 0.9f);
					p2Text.text = "Ready!";
					p2IsIn = true;
					if (p1IsIn) {
						ReadyText.text = "Press Start";
					}
				}
			}
			break;


		}
	}

	public void SetSelectedButton(int buttonId){
		switch (state) {
		case MenuState.Main:
			for (int i = 0; i < mainButtons.Length; i++) {
				if (i == buttonId) {
					mainButtons [i].gameObject.GetComponent<Image> ().color = mainButtons [i].colors.highlightedColor;
				} else {
					mainButtons [i].gameObject.GetComponent<Image> ().color = mainButtons [i].colors.normalColor;
				}
			}
			break;

		case MenuState.StageSelect:
			for (int i = 0; i < stageButtons.Length; i++) {
				if (i == buttonId) {
					stageButtons [i].gameObject.GetComponent<Image> ().color =  stageButtons[i].colors.highlightedColor;
				} else {
					stageButtons[i].gameObject.GetComponent<Image> ().color =  stageButtons[i].colors.normalColor;
				}
			}
			break;
		}
	}

	public void GoToInstructions(){
		state = MenuState.Instructions;
		Main.SetActive (false);
		mainButtons [selectedButton].gameObject.GetComponent<Image> ().color = mainButtons [selectedButton].colors.normalColor;
		selectedButton = 0;
		SetSelectedButton (0);
		CoverScreen.gameObject.SetActive (true);
		BackText.gameObject.SetActive (true);
	}

	public void GoToStageSelect(){
		state = MenuState.StageSelect;
		Main.SetActive (false);
		mainButtons [selectedButton].gameObject.GetComponent<Image> ().color = mainButtons [selectedButton].colors.normalColor;
		selectedButton = 0;
		CoverScreen.gameObject.SetActive (true);
		BackText.gameObject.SetActive (true);
		StageSelect.SetActive (true);
	}

	public void GoToPlayerConfirm(){
		if (selectedButton == 0) {
			stageSelected = "City";
		} else if (selectedButton == 1) {
			stageSelected = "Desert";
		}

		state = MenuState.PlayerConfirm;
		StageSelect.SetActive (false);
		stageButtons[selectedButton].gameObject.GetComponent<Image> ().color =  stageButtons[selectedButton].colors.normalColor;
		selectedButton = 0;
		PlayerConfirm.SetActive (true);
	}

	public void Exit(){
		Application.Quit ();
	}
}
