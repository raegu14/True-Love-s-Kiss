using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class FinalScript : MonoBehaviour {

	public GameObject[] boss;
	bool activeBoss = true;
	public Sprite bg;
	int countdown = 300;
	bool count = false;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(boss.Length != 0 || activeBoss)
		{
			activeBoss = false;
			foreach (GameObject b in boss)
			{
				if(b != null)
				{
					activeBoss = true;
				}
			}
		}
		if(!activeBoss)
			GetComponent<Interactable>().Interact();
		if(count)
		{
			if(countdown > 0)
				countdown--;
			else
				GameObject.Find("Main Camera").GetComponent<Menu>().FadeOut(false, "End");
		}
	}

	[YarnCommand("setbg")]
	public void SetBG()
	{
		GameObject.Find("Player").GetComponent<Player>().SetActive(false);
		GetComponent<SpriteRenderer>().sprite = bg;
		transform.position = GameObject.Find("Main Camera").transform.position;
		transform.position += new Vector3(0, 0, 10f);
		count = true;
	}
}
