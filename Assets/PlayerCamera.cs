using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	public Transform target;
	public float zDistance;
	public float yDistance;
	public float xDistance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position.Set (target.position.x - xDistance, target.position.y - yDistance, target.position.z - zDistance);
	}

	public void SetViewPort(float x, float y, float w, float h){
		this.GetComponent<Camera> ().rect = new Rect (x, y, w, h);
	}

}
