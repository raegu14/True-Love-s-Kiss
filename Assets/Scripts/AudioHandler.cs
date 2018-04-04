using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour {

	[Header("AudioSources")]
	public AudioSource main;
	public AudioSource aux1;
	public AudioSource aux2;
	public AudioSource ambient1;
	public AudioSource ambient2;
	public AudioSource persistent1;
	public AudioSource persistent2;
	public AudioSource random1;
	public AudioSource random2;
	[Header("BGM")]
	public AudioClip TitleScreen;
	public AudioClip KnightBGM;
	public AudioClip BossBGM;
	public AudioClip gameOver;
	[Header("Sound Effects")]
	public AudioClip reflector;
	public AudioClip attackYell;
	public AudioClip attackSword;
	public AudioClip attackStaff;
	public AudioClip jump;
	public AudioClip struck;
	public AudioClip heal;
	public AudioClip enemyStruck;
	public AudioClip magic;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void FixedUpdate () {
		// random sounds
	}

	public void Updoot()
	{
		string namae = SceneManager.GetActiveScene().name;
		if(namae == "Title" || namae == "End")
		{
			main.clip = TitleScreen;
			main.Play();
		}
		else
		{
			if(main.clip != KnightBGM)
			{
				main.clip = KnightBGM;
				main.Play();
			}
		}

	}

	public void Play(string name)
	{
		if(name == "reflect")
		{
			aux1.clip = reflector;
			aux1.Play();
			Debug.Log("reflect");
		}
		else if(name == "yell")
		{
			if(!aux2.isPlaying)
			{
				aux2.clip = attackYell;
				aux2.Play();
			}
		}
		else if(name == "struck")
		{
			aux1.clip = struck;
			aux1.Play();
		}
		else if(name =="sword")
		{
			random1.clip = attackSword;
			random1.Play();
		}
		else if(name == "staff")
		{
			random1.clip = attackStaff;
			random1.Play();
		}
		else if(name == "heal")
		{
			aux2.clip = heal;
			aux2.Play();
		}
		else if(name == "enemystruck")
		{
			random2.clip = enemyStruck;
			random2.Play();
		}
		else if(name == "magic")
		{
			ambient1.clip = magic;
			ambient1.Play();
		}
		else if(name == "die")
		{
			main.clip = gameOver;
			main.Play();
		}
		else if(name == "jump")
		{
			ambient2.clip = jump;
			ambient2.Play();
		}
	}

	public void Stop(string name)
	{
	}
}
