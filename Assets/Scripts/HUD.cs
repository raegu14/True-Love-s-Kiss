using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	// Script references
	Player player;
	Meta meta;

	// Instance data
	int currHP;
	int maxHP;
	float swordCD, staffCD;
	float dashCD;
	string currentEquip;
	bool shownSword, shownStaff;

	[Header("HUD Pieces")]
	public GameObject healthBar;
	public GameObject swordMain;
	public GameObject swordSub;
	public GameObject staffMain;
	public GameObject staffSub;
	SpriteRenderer swordMainSR, staffMainSR, swordSubSR, staffSubSR;
	Color increment;
	public Color subColor = Color.black;
	public Color mainColor = Color.black;
	float flashSword = 0, flashStaff = 0;
	public float flashTime = 0.25f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").GetComponent<Player>();
		meta = GameObject.Find("META").GetComponent<Meta>();
		currHP = meta.GetPlayerHP();
		maxHP = meta.GetMaxHP();
		dashCD = player.dashCooldown;
		float temp = 0.5f/dashCD*Time.deltaTime;
		Debug.Log(temp);
		increment = new Color(temp, temp, temp, 0f);
		if(mainColor == Color.black)
			mainColor = Color.green;
		if(subColor == Color.black)
			subColor = new Color(mainColor.r, mainColor.g, mainColor.b, 0.5f);
		InitWeapon();
		SetHP(currHP);
	}

	// Update is called once per frame
	void Update () {
		if(currHP != player.hp)
			SetHP(player.hp);
		if(currentEquip != player.currentWeapon)
		{
			if(player.hasSword && player.hasStaff)
				SwitchWeapon();
			else if(player.hasSword)
				ShowSword(true);
			else
				ShowStaff(true);
			currentEquip = player.currentWeapon;
		}
		float swordcd = player.hDashCD > 0 ? player.hDashCD : 0;
		float staffcd = player.vDashCD > 0 ? player.vDashCD : 0;
		if(swordcd == 0)
		{
			if(flashSword > 0)
			{
				flashSword -= Time.deltaTime;
			}
			else
			{
				swordMainSR.color = mainColor;
				swordSubSR.color = subColor;
			}
		}
		else if(swordcd == dashCD)
		{
			swordMainSR.color = Color.gray;
			swordSubSR.color = Color.gray;
			flashSword = flashTime;
		}
		else if(swordcd < dashCD/2)
		{
			swordMainSR.color += increment;
			swordSubSR.color += increment;
		}
		if(staffcd == 0)
		{
			if(flashStaff > 0)
			{
				flashStaff -= Time.deltaTime;
			}
			else
			{
				staffMainSR.color = mainColor;
				staffSubSR.color = subColor;
			}
		}
		else if(staffcd == dashCD)
		{
			staffMainSR.color = Color.gray;
			staffSubSR.color = Color.gray;
			flashSword = flashTime;
		}
		else if(staffcd < dashCD/2)
		{
			staffMainSR.color += increment;
			staffSubSR.color += increment;
		}
	}

	public void SetHP(int hp)
	{
		currHP = hp > maxHP ? maxHP : hp;
		for(int i = 0; i < maxHP; i++)
		{
			if(i < currHP)
			{
				healthBar.transform.GetChild(i).gameObject.SetActive(true);
			}
			else
			{
				healthBar.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}

	public void InitWeapon()
	{
		swordSubSR = swordSub.GetComponent<SpriteRenderer>();
		swordMainSR = swordMain.GetComponent<SpriteRenderer>();
		staffSubSR = staffSub.GetComponent<SpriteRenderer>();
		staffMainSR = staffMain.GetComponent<SpriteRenderer>();

		currentEquip = meta.GetCurrentWeapon();
		if(meta.GetWeaponStatus("sword"))
		{
			if(currentEquip == "Sword")
			{
				ShowSword(true);
				currentEquip = "Sword";
			}
			else
			{
				ShowSword(false);
			}
		}
		if(meta.GetWeaponStatus("staff"))
		{
			if(currentEquip == "Staff")
			{
				ShowStaff(true);
				currentEquip = "Staff";
			}
			else
			{
				ShowStaff(false);
			}
		}
		swordSubSR.color = subColor;
		swordMainSR.color = mainColor;
		staffSubSR.color = subColor;
		staffMainSR.color = mainColor;
	}

	// Assumes you own sword
	public void ShowSword(bool equipped)
	{
		swordMain.SetActive(equipped);
		swordSub.SetActive(!equipped);
	}

	// Assumes you own staff
	public void ShowStaff(bool equipped)
	{
		staffMain.SetActive(equipped);
		staffSub.SetActive(!equipped);
	}

	// Assumes you can switch weaposn in the first place
	public void SwitchWeapon()
	{
		if(currentEquip == "Sword" && player.hasStaff)
		{
			ShowStaff(true);
			ShowSword(false);
			currentEquip = "Staff";
		}
		else if(currentEquip == "Staff" && player.hasSword)
		{
			ShowStaff(false);
			ShowSword(true);
			currentEquip = "Sword";
		}
		else
			Debug.Log("You forgot to check you could switch weapons");
	}
}
