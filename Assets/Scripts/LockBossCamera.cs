using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBossCamera : MonoBehaviour {

    public GameObject cam;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            cam.GetComponent<SmoothFollow>().leftClamp = 36;
        }
           
    }
}
