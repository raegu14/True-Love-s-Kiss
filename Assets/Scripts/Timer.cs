using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE just a script to help time things for debugging
public class Timer : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player")
		{
			Debug.Log(Time.realtimeSinceStartup);
		}
	}
}
