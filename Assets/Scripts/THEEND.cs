using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class THEEND : MonoBehaviour {

  float countdown = 5f;
  public Texture2D fadeTexture;
  float fadeSpeed = 0.75f;
  public int drawDepth = -1000;
  float alpha = 0f;
	bool fade = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(countdown <= 0)
		{
			fade = true;
		}
		else
			countdown -= Time.deltaTime;
	}

	void OnGUI()
	{
		// Fade to black and restart level
		if(fade)
		{
			// player no longer affects surroundings
			alpha += fadeSpeed * Time.deltaTime;
			Color c = GUI.color; c.a = alpha;
			GUI.color = c;
			GUI.depth = drawDepth;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
			if(c.a >= 1f)
			{
					SceneManager.LoadScene("Title");
			}
		}
	}
}
