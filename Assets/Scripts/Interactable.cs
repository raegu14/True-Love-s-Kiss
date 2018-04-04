using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Interactable : MonoBehaviour {

	public GameObject indicator;
	public bool talkOnCollision = true;
	public bool repeatable = false;
	bool final = false;
	int repetitions = 0;

	// Use this for initialization
	void Start () {
		if(GetComponent<FinalScript>() != null)
			final = true;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(!final)
		{
			if(col.gameObject.name == "Player")
			{
				col.gameObject.GetComponent<Player>().ChangeInteract(gameObject);
				if(talkOnCollision)
					Interact();
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(!final)
		{
			if(col.gameObject.name == "Player")
			{
				Player p = col.gameObject.GetComponent<Player>();
				if(p.toInteract == gameObject)
					p.ChangeInteract(null);
			}

		}
	}

	public void Interact()
	{
		if(repeatable || repetitions == 0)
		{
			repetitions++;
			if(gameObject.tag == "Prince")
			{
				DialogueRunner d = GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>();
				d.StartDialogue(gameObject.transform.name + "_Start");
				Player p = GameObject.Find("Player").GetComponent<Player>();
				p.SetActive(false, true);
			}
			talkOnCollision = false;
		}
	}

	public void ShowIndicator()
	{
		if(indicator != null)
			indicator.SetActive(true);
	}

	public void HideIndicator()
	{
		if(indicator != null)
			indicator.SetActive(false);
	}
}
