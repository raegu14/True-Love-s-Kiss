using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Activatable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			if(child.GetComponent<SpriteRenderer>())
				child.GetComponent<SpriteRenderer>().enabled = false;
			else
				child.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	[YarnCommand("activate")]
	public void Activate()
	{
		foreach (Transform child in transform)
		{
			if(child.GetComponent<SpriteRenderer>())
				child.GetComponent<SpriteRenderer>().enabled = true;
			else
				child.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
