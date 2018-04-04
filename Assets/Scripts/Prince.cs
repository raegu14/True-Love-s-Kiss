using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Prince : MonoBehaviour {

	// Please name the associated game object with whatever the prince's name is
	// Please name all associated resources like name_assetType - for example:
		// Alejandro_portrait.png
		// Alejandro_dialog.json

	public TextAsset dialog;

	// Use this for initialization
	void Start () {
		ReadInfo();
	}

	// Update is called once per frame
	void Update () {

	}

	void ReadInfo()
	{
		string stem = transform.name;

		// Dialog
		if(dialog == null)
			dialog = (TextAsset)Resources.Load(stem+"_dialog", typeof(TextAsset));
	}

}
