using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class Meta : MonoBehaviour {

	[Header("Save Data")]
	// Recruitment is updated immediately upon recruitment
	// everything else is updated upon leaving an area
	public static bool[] recruitedPrinces;
	public static int playerHP = 0; //current hp
	public static int playerMaxHP = 0;
	public static string currentLevel;
	public static string lastLevel;
	public static GameObject[] inventory;
	public static bool hasSword;
	public static bool hasStaff;
	public static string currentWeapon;
	static VariableStorage dialogueVars;

	[Header("Instance Data")]
	static AudioHandler audHandler;
	static Player player;
	static DialogueRunner dialogue;
	public static Dictionary<string, int> princeLookup;
	static bool setup;

	GameObject[] spawnPoints;

	// Make sure there aren't any duplicates
	static GameObject instance;

  void Awake() {
		if(instance != null)
		{
			instance.GetComponent<Meta>().Setup();
			Destroy(gameObject);
			return;
		}
		instance = gameObject;
    DontDestroyOnLoad(gameObject);
		// Initialize dictionary
		if(princeLookup == null)
		{
			princeLookup = new Dictionary<string, int>()
			{
				{"Example1", 0},
				{"Example2", 1}
			};
			recruitedPrinces = new bool[princeLookup.Count];
		}
		Setup();
  }

	public void Setup()
	{
		Debug.Log("Set up level");
		if(audHandler == null)
		{
			audHandler = GameObject.FindWithTag("AudioHandler").GetComponent<AudioHandler>();
		}
		audHandler.Updoot();
		player = GameObject.Find("Player").GetComponent<Player>();

		if(player != null)
		{
			if(playerMaxHP != 0)
				player.maxHP = playerMaxHP;
			else
				playerMaxHP = player.maxHP;

			if(playerHP != 0)
				player.hp = playerHP;
			else
				playerHP = player.hp;

			if(currentWeapon != null)
				player.currentWeapon = currentWeapon;
			else
				currentWeapon = player.currentWeapon;

			if(setup)
			{
				player.hasStaff = hasStaff;
				player.hasSword = hasSword;
			}
			else
			{
				hasStaff = player.hasStaff;
				hasSword = player.hasSword;
			}
			setup = true;
			currentLevel = SceneManager.GetActiveScene().name;
			spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
			foreach (GameObject spawn in spawnPoints)
			{
				if(spawn.transform.name == lastLevel)
				{
					player.transform.position = spawn.transform.position;
					return;
				}
			}
		}
	}


	public void End()
	{
		// TODO end game
	}


	public void Updata()
	{
		// TODO keep track of where you are
		playerMaxHP = player.maxHP;
		playerHP = player.hp;
		lastLevel = currentLevel;
		if(player.currentWeapon != null)
		{
			currentWeapon = player.currentWeapon;
		}
		hasSword = player.hasSword;
		hasStaff = player.hasStaff;

	}

	public int GetPlayerHP()
	{
		return playerHP;
	}
	public int GetMaxHP()
	{
		return playerMaxHP;
	}

	public string GetCurrentWeapon()
	{
		return currentWeapon;
	}
	public bool GetWeaponStatus(string weapon)
	{
		if(weapon == "staff")
		{
			return hasStaff;
		}
		if(weapon == "sword")
		{
			return hasSword;
		}
		return false;
	}
}
