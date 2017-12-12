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
		/*
		float targetaspect = 8.0f / 10.0f;

		float rectX = 0f;

		if (this.gameObject.layer == LayerMask.NameToLayer("p2")){
			rectX = 0.5f;
		}

		float windowAspect = (float)Screen.width / (float)Screen.height;

		// current viewport height should be scaled by this amount
		float scaleH = windowAspect / targetaspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if scaled height is less than current height, add letterbox
		if (scaleH < 1.0f)
		{  
			Rect rect = camera.rect;

			rect.width = 0.5f;
			rect.height = scaleH;
			rect.x = rectX;
			rect.y = (1.0f - scaleH) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scaleW = 0.5f / scaleH;

			Rect rect = camera.rect;

			rect.width = scaleW;
			rect.height = 1.0f;
			rect.x = (1.0f - scaleW) / 2.0f + rectX;
			rect.y = 0;

			camera.rect = rect;
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position.Set (target.position.x - xDistance, target.position.y - yDistance, target.position.z - zDistance);
	}

	public void SetViewPort(float x, float y, float w, float h){
		this.GetComponent<Camera> ().rect = new Rect (x, y, w, h);
	}

}
