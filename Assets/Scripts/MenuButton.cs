using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuButton : MonoBehaviour {

	public Menu menu;
	public Color original = Color.white;
	public Color hover = Color.red;
	public string action;
	public GameObject pointer;
	SpriteRenderer text;

	// Use this for initialization
	void Start () {
		text = GetComponent<SpriteRenderer>();
	}

	void OnMouseOver()
	{
		text.color = hover;
		//pointer.SetActive(true);
		if(Input.GetMouseButtonDown(0))
		{
			if(action == "restart")
			{
				menu.Restart();
			}
			else if(action == "resume")
			{
				menu.Unpause();
			}
			else if(action == "title")
			{
				menu.TitleScreen();
			}
			else if(action == "quit")
			{
				Application.Quit();
			}
			else if(action == "newgame")
			{
				Debug.Log("New Game -- better implement it lol"); // TODO
				SceneManager.LoadScene("Base");
				//SceneManager.LoadScene("1-1");
				// TODO make sure it loads/clears properly
			}
			else if(action == "loadgame")
			{
				Debug.Log("Load Game -- better implement it lol"); // TODO
			}
		}
	}

	void OnMouseExit()
	{
		text.color = original;
		//pointer.SetActive(false);
	}
}
