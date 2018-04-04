using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

// Attach this to the main camera
public class Menu : MonoBehaviour {

	[Header("Menu")]
	public GameObject menu;
	public GameObject events;
	public EventSystem menuSystem;
	public GameObject emptyOption;
	public GameObject menuOptions;
	public GameObject controls;
	public GameObject gameover;
	public bool paused = false;
	bool controlsActive = false;
	public bool pausible = true;

	// Player
	Player player;

  // Fade out
	[Header("Fade")]
  public Texture2D fadeTexture;
  float fadeSpeed = 0.75f;
  public int drawDepth = -1000;
  float alpha = 0f;
  bool fade = false;
	bool restart = true;

	bool inputReset = true;
	int controlsTimer = 0;
	string nextLevelName;
	bool buttonReset = false;


	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		if(events == null)
			events = GameObject.Find("EventSystem");
	}

	// Update is called once per frame
	void Update () {
		if(buttonReset && Input.GetAxisRaw("Pause") > 0)
		{
			if(pausible)
			{
				if(!paused)
					Pause();
				else
					Unpause();
			}
			buttonReset = false;
		}
		else if(paused)
		{
			if(menuSystem.currentSelectedGameObject == null)
				menuSystem.SetSelectedGameObject(emptyOption);
		}
		if(Input.GetAxisRaw("Pause") == 0)
			buttonReset = true;
		if(controlsActive && controlsTimer <= 0)
		{
			if(Input.GetMouseButtonDown(0) || Input.GetAxisRaw("Interact") > 0 && inputReset)
			{
				controls.SetActive(false);
				menuOptions.SetActive(true);
				inputReset = false;
			}
			else if(!inputReset && Input.GetAxisRaw("Interact") == 0)
				inputReset = true;
		}
		else if(controlsTimer > 0)
		{
			controlsTimer--;
		}
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
				if(restart)
					Restart();
				else
					SceneManager.LoadScene(nextLevelName);
			}
		}
	}

	public void Quit()
	{
		Debug.Log("quit");
		Application.Quit();
	}

	public void Pause()
	{
		Time.timeScale = 0;
		paused = true;
		events.SetActive(false);
		menu.SetActive(true);
		player.SetActive(false);
	}
	public void Unpause()
	{
		Time.timeScale = 1;
		paused = false;
		pausible = true;
		controls.SetActive(false);
		menu.SetActive(false);
		events.SetActive(true);
		player.SetActive(true);
	}
	public void Restart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void TitleScreen()
	{
		foreach(GameObject m in GameObject.FindGameObjectsWithTag("meta"))
		{
			// TODO saving and stuff
			Destroy(m);
		}
		// TODO load title
		SceneManager.LoadScene("Title");
	}
	public void Save()
	{
		// TODO save game
	}

	public void NewGame()
	{
		SceneManager.LoadScene("0-1");
	}

	public void ShowControls()
	{
		if(controls == null)
			controls = GameObject.Find("ControlScheme");
		if(menuOptions == null)
			menuOptions = GameObject.Find("MenuOptions");
		menuOptions.SetActive(false);
		controls.SetActive(true);
		controlsTimer = 50;
		controlsActive = true;
	}

	public void ShowCredits()
	{
		Debug.Log("show credits");
	}

	public void ReturnToMenu()
	{

	}

	public void FadeOut(bool re, string next)
	{
		GameObject.Find("Player").GetComponent<Player>().SetActive(false);
		fade = true;
		restart = re;
		nextLevelName = next;
	}

	public void GameOver()
	{
		Time.timeScale = 0;
		paused = true;
		events.SetActive(false);
		gameover.SetActive(true);
		player.SetActive(false);

	}

}
