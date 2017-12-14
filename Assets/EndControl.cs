using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndControl : MonoBehaviour {
	public Text MenuText;
	public float BlinkOutTime;
	public float BlinkInTime;

	private float blinkTimer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		blinkTimer += Time.deltaTime;
		if (MenuText.color.a == 1.0f) {
			if (blinkTimer >= BlinkOutTime) {
				MenuText.color = new Color (0f, 0f, 0f, 0f);
				blinkTimer = 0;
			}
		} else {
			if (blinkTimer >= BlinkInTime) {
				MenuText.color = new Color (0f, 0f, 0f, 1f);
				blinkTimer = 0;
			}
		}

		if (Input.GetKey (KeyCode.Return) || Input.GetButton("Start1") || Input.GetButton("Start2")) {
			SceneManager.LoadScene ("TitleMenu");
		}
	}
}
