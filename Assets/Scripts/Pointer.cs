using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {

	Camera cam;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
		pos.z += 1f;
		transform.position = pos;
	}
}
