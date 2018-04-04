using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Projectile proj = col.gameObject.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.direction = -proj.direction;
            //proj.refle
        }
    }
}
