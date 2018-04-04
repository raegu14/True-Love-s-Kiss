using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supermeta : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if(GameObject.FindGameObjectsWithTag("meta").Length > 3)
			Destroy(this.gameObject);
		DontDestroyOnLoad(transform.gameObject);
	}
}
