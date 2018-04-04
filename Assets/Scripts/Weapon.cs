using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	Player player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			Debug.Log("hit");
			col.gameObject.GetComponent<Enemy>().TakeDamage(player.damage);
		}
	}

		void OnCollisionEnter2D(Collision2D col)
		{
			Debug.Log(col.gameObject.tag);
			if(col.gameObject.tag == "Enemy")
			{
				Debug.Log("hit");
				col.gameObject.GetComponent<Enemy>().TakeDamage(player.damage);
			}
		}
}
