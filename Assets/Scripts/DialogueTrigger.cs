using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(BoxCollider2D))]
public class DialogueTrigger : MonoBehaviour {

	public string nodeName;
	DialogueRunner d;

	// Use this for initialization
	void Start () {
		d = GameObject.Find("DialogueRunner").GetComponent<DialogueRunner>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			d.StartDialogue(nodeName);
		}
	}
}
