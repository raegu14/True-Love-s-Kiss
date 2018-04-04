using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class Player : MonoBehaviour {

	// HP
	[Header("HP")]
	public int hp = 6; // max 20;
	public int maxHP = 6;

	// movement
	[Header("Movement")]
	bool talking = false;
	public bool active = true;
	public float moveSpeed = 5f;
	public char lastHorizontal;
	Vector2 slope = Vector2.zero;

	// jump
	public float jumpSpeed = 20f;
	public float dropDelay = 0.5f;
	public float MARGIN = 0.1f; // raycast wiggle room
	public int numJumps = 2;
	float jumpDelay = 0.1f; // make sure you get off the ground before checking for grounding
	bool jumping = false;
	bool rising = false;
	float grav;
	float JUMP_DELAY;
	float DROP_DELAY;
	int NUM_JUMPS;
	int layerMask;
	Vector2 diag1, diag2, offset1, offset2, offsetl, offsetr;

	// gliding
	public float glideSpeed = 1f;

	// interactions
	public GameObject toInteract;
	public GameObject oneWayPlatform;

	//weapons -- TODO start with no weapons
	[Header("Weapons")]
	public GameObject sword;
	public GameObject staff;
	public bool hasSword = true;
	public bool hasStaff = true;
	public string currentWeapon = "Sword";
	public bool dashing = false;    //needed to kill enemies
	public bool dashUp = false;     //needed to kill enemies
	public float verticalDashForce = 30f;
	public float horizontalDashForce = 20f;
	public float verticalDashTime = 0.25f;
	public float horizontalDashTime = 0.25f;
	float dashTimer = 0f;
	public float dashCooldown = 1f;
	public float vDashCD = 0;
	public float hDashCD = 0;
	public int damage = 1;

	bool attacking = false;
	bool switchQ = false;

	//components
	Rigidbody2D rb;
	BoxCollider2D col;

	//animation
	SpriteRenderer sr;
	Animator anim;
	bool isWalking;
	bool left;
	float flashTimer = 0;
	int flicker = 0;

	// Other stuff
	[Header("Other Things")]
	public Meta meta;
	public AudioHandler aud;
	//public HUD hud;

	//controller resets
	bool weaponReset = true;
	bool JumpRelease;
	bool groundJumpReset;
	bool interactReset = true;
	public bool attackReset = true;

	//particles
	float PARTICLE_PROBABILITY = 0.8f;
	public GameObject particles;


	// Use this for initialization
	void Start () {
		// Grab components
		sr = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		col = GetComponent<BoxCollider2D>();
		// Set up constants
		JUMP_DELAY = jumpDelay;
		NUM_JUMPS = numJumps;
		DROP_DELAY = dropDelay;
		jumpDelay = 0;
		dropDelay = 0;
		// Set up grounding checks
		diag1 = new Vector2(-1, -1); offset1 = col.bounds.center - col.bounds.extents;
		diag2 = new Vector2(1, -1); offset2 = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y);
		grav = rb.gravityScale;
		layerMask = LayerMask.GetMask("Ground");
		// Wall checks
		offsetl = new Vector2(-col.bounds.size.x, -col.bounds.extents.y);
		offsetr = new Vector2(col.bounds.size.x, -col.bounds.extents.y);
		//offsetl = Vector2.Scale(Vector2.left, col.bounds.size) + ;
		//offsetr = Vector2.Scale(Vector2.right, col.bounds.size);

		meta = GameObject.Find("META").GetComponent<Meta>();
		meta.Setup();
		aud = GameObject.Find("Audio").GetComponent<AudioHandler>();
		Debug.Log(hp);
        //hud = GameObject.Find("HUD").GetComponent<HUD>();

        //set up xbox controller
        if (hasStaff)
        {
            anim.SetInteger("Weapon", 2);
        }
        else
        {
            anim.SetInteger("Weapon", 0);
        }
	}

	void FixedUpdate()
	{
		if(flashTimer > 0)
		{
			flashTimer -= Time.deltaTime;
			flicker++;
			if(flicker%7 == 0)
				sr.enabled = !sr.enabled;
			if(flashTimer < 1.25f && !active && !talking)
			{
				active = true;
			}
		}
		else
		{
			if(!sr.enabled)
				sr.enabled = true;
			if(gameObject.layer != 9)
				gameObject.layer = 9;
		}
	}

	// Update is called once per frame
	// NOTE -- DO NOT MAKE THIS INTO FIXED UPDATE
	//   - FIXED UPDATE WILL MESS WITH KEY INPUT DETECTION
	// NOTE -- the order of this stuff has PROBABLY been thought out by Lucy
	//   - changing execution order of if statements MAAAAY screw things up
	void Update () {

		//spawn particles

		if (Random.value < PARTICLE_PROBABILITY)
		{
			GameObject temp = Instantiate(particles);
			temp.GetComponent<Particles>().SetBound(sr.bounds.extents);
			temp.GetComponent<Particles>().Init(gameObject, 0.5f, 5f, 2f);
		}

		if(active)
		{
			if(hDashCD > 0)
				hDashCD -= Time.deltaTime;
			if(vDashCD > 0)
				vDashCD -= Time.deltaTime;
			// Handle movement
			float xOffset = 0; float yOffset = 0;
			if(switchQ || Input.GetAxisRaw("Switch") > 0 && weaponReset) // switch weapon
			{
				if(!attacking)
				{
					switchQ = false;
					weaponReset = false;
					if(hasSword && currentWeapon == "Sword")
					{
						if (hasStaff)
						{
							currentWeapon = "Staff";
							anim.SetInteger("Weapon", 2);
						}
					}
					else if(hasStaff && currentWeapon == "Staff")
					{
						if(hasSword)
						{
							currentWeapon = "Sword";
							anim.SetInteger("Weapon", 1);
						}
					}
				}
				else
					switchQ = true;
			}
			if (Input.GetAxisRaw("Switch") == 0)
			{
				weaponReset = true;
			}
			if(!dashing)
			{
				if(Input.GetAxisRaw("Alternate Attack") < 0)
				{
					if(hasSword && currentWeapon == "Sword" && hDashCD <= 0)
					{
            anim.SetTrigger("Dash");
            dashing = true;
						rb.gravityScale = 0f;
						dashUp = false;
						dashTimer = horizontalDashTime;
						hDashCD = dashCooldown;
						rb.velocity = Vector2.right * (lastHorizontal == 'l' ? -1 : 1) * horizontalDashForce;
					}
					else if(hasStaff && currentWeapon == "Staff" && vDashCD <= 0)
					{
            anim.SetTrigger("Dash");
            dashing = true;
						rb.gravityScale = grav;
						dashUp = true;
						dashTimer = verticalDashTime;
						vDashCD = dashCooldown;
						rb.velocity = Vector2.up * verticalDashForce;
					}
					if(dashing)
						return;
				}
			}
			else
			{
				if(dashTimer > 0f)
				{
					dashTimer -= Time.deltaTime;
					return;
				}
				else
				{
					rb.gravityScale = grav;
					dashing = false;
					rb.velocity = new Vector2(0, rb.velocity.y);
				}
			}

			if (Input.GetAxisRaw("Horizontal") < 0)
			{
				lastHorizontal = 'l';
			}
			else  if(Input.GetAxisRaw("Horizontal") > 0)
			{
				lastHorizontal = 'r';
			}

			Vector2 pos = new Vector2(transform.position.x, transform.position.y);
			offset1 = col.bounds.min;
			offset2 = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y);
			if (Input.GetAxisRaw("Horizontal") < 0)
			{
				anim.SetBool("Move", true);
				Vector3 scale = transform.localScale;
				scale.x = -0.5f;
				transform.localScale = scale;
				RaycastHit2D hit = Physics2D.Raycast(pos + offsetl, Vector2.left, 3*MARGIN, layerMask);
				Debug.DrawRay(pos + offsetl, Vector2.left, Color.green);
				if (hit.collider == null || hit.normal.x == 0)
				{
					if (lastHorizontal == 'l' || !(Input.GetAxisRaw("Horizontal") > 0))
						xOffset -= moveSpeed * Time.deltaTime;
					if (slope.y > 0 && slope.x != 0)
					{
						if (slope.x < 0)
							yOffset += Mathf.Abs(moveSpeed * slope.y / slope.x * Time.deltaTime);
						else
							yOffset -= Mathf.Abs(moveSpeed * slope.y / slope.x * Time.deltaTime);
					}
				}
			}

			else if (Input.GetAxisRaw("Horizontal") > 0)
			{
				anim.SetBool("Move", true);
				Vector3 scale = transform.localScale;
				scale.x = 0.5f;
				transform.localScale = scale;
				RaycastHit2D hit = Physics2D.Raycast(pos + offsetr, Vector2.right, 3*MARGIN, layerMask);
				Debug.DrawRay(pos + offsetr, Vector2.Scale(Vector2.right, new Vector2(3*MARGIN, 3*MARGIN)), Color.red);
				if (hit.collider == null || hit.normal.x == 0)
				{
					if (lastHorizontal == 'r' || !(Input.GetAxisRaw("Horizontal") < 0))
						xOffset += moveSpeed * Time.deltaTime;
					if (slope.y > 0 && slope.x != 0)
					{
						if (slope.x > 0)
							yOffset += Mathf.Abs(moveSpeed * slope.y / slope.x * Time.deltaTime);
						else
							yOffset -= Mathf.Abs(moveSpeed * slope.y / slope.x * Time.deltaTime);
					}
				}
			}
			else
			{
				anim.SetBool("Move", false);
			}

			if (Input.GetAxisRaw("Jump") > 0 && numJumps > 0 && JumpRelease)
			{
				rb.gravityScale = grav;
				rb.velocity = new Vector2(0, jumpSpeed);
				numJumps--;
				jumping = true;
				slope = Vector2.zero;
				groundJumpReset = false;
				anim.SetTrigger("Jump");
				aud.Play("jump");
			}
			else if (Input.GetAxisRaw("Jump") > 0 && !JumpRelease && !groundJumpReset)
			{
				// glide
				rb.gravityScale = grav;
				if (-rb.velocity.y > glideSpeed)
				{
					rb.velocity = new Vector2(rb.velocity.x, -glideSpeed);
				}
				anim.SetBool("Gliding", true);
			}
			if (Input.GetAxisRaw("Jump") == 0)
			{
				JumpRelease = true;
				anim.SetBool("Gliding", false);
			}
			else
			{
				JumpRelease = false;
			}

			anim.SetBool("Ground", false);
			if (jumping && jumpDelay > 0)
			{
				jumpDelay -= Time.deltaTime;
			}
			else if(dropDelay > 0)
			{
				dropDelay -= Time.deltaTime;
			}
			else if(IsGrounded())
			{
				ResetJump();
				anim.SetBool("Ground", true);
				rb.velocity = Vector2.zero;
			}
			else// not grounded
			{
				rb.gravityScale = grav;
			}
			transform.position += new Vector3(xOffset, yOffset, 0);

			if(rb.velocity.y > 0)
				rising = true;
			else
				rising = false;


			// interactions
			if(Input.GetAxisRaw("Interact") > 0 && interactReset)
			{
				interactReset = false;
				if(toInteract != null)
				{
					toInteract.GetComponent<Interactable>().Interact();
				}
			}
			if(Input.GetAxisRaw("Interact") == 0)
			{
				interactReset = true;
			}
			// Attacking
			if(Input.GetAxisRaw("Normal Attack") > 0 && attackReset)
			{
				int prevAnim = anim.GetCurrentAnimatorStateInfo(0).shortNameHash;
				if(hasSword || hasStaff)
				{
					anim.SetTrigger("Attack");
					attackReset = false;
					attacking = true;
					ActivateWeapon();
				}
			}
			if(Input.GetAxisRaw("Normal Attack") == 0)
			{
				attackReset = true;
			}
			if(!anim.GetBool("Attack") &&
				!(anim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack")
					|| anim.GetCurrentAnimatorStateInfo(0).IsName("StaffAttack")))
				DeactivateWeapon();

			if(Input.GetAxisRaw("Vertical") < 0)
			{
				if(oneWayPlatform != null)
				{
					oneWayPlatform.GetComponent<OneWay>().TempDisable();
					oneWayPlatform = null;
					rb.gravityScale = grav;
					dropDelay = DROP_DELAY;
				}
			}
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		// TODO reset control when vertical dashing into the ceiling?
		// idk
	}

	bool IsGrounded()
	{
		/*
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, sr.bounds.extents,
		0, Vector2.down, distToGround/2 + MARGIN, layerMask);
		*/
		if(rising)
			return false;
		// NOTE -- WILL BE BUGGY FOR VERY NARROW PLATFORMS
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		RaycastHit2D hit1 = Physics2D.Raycast(offset1, diag1, MARGIN, layerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(offset2, diag2, MARGIN, layerMask);
		if(hit1.collider == null && hit2.collider == null)
			return false;

		if(hit1.collider != null && hit1.collider.tag == "OneWay")
		{
			oneWayPlatform = hit1.collider.gameObject;
		}
		else if(hit2.collider != null && hit2.collider.tag == "OneWay")
		{
			oneWayPlatform = hit2.collider.gameObject;
		}
		else
		{
			oneWayPlatform = null;
		}

		if(hit1.collider != null)
		{
			slope = new Vector2(hit1.normal.y, -hit1.normal.x);
			if(hit1.normal.y == 0)
				return false;
			if(hit2.collider != null)
			{
				slope = Vector2.zero;
				return true;
			}
		}
		if(hit2.collider != null)
		{
			slope = new Vector2(hit2.normal.y, -hit2.normal.x);
			if(hit2.normal.y == 0)
				return false;
		}
		if(slope.y < 0)
		{
			slope = new Vector2(-slope.x, -slope.y);
		}
		return true;
	}

	void ResetJump()
	{
		jumpDelay = JUMP_DELAY;
		numJumps = NUM_JUMPS;
		jumping = false;
		rb.gravityScale = 0;
		groundJumpReset = true;
		rb.velocity = Vector3.zero;
		//JumpRelease = true;
	}

	public void SetActive(bool a)
	{
		active = a;
		anim.SetBool("Move", false);
	}

	public void SetActive(bool a, bool t)
	{
		talking = t;
		SetActive(a);
	}

	public void ChangeInteract(GameObject obj)
	{
		if(toInteract != obj)
		{
			if(toInteract != null)
				toInteract.GetComponent<Interactable>().HideIndicator();
			if(obj != null)
				obj.GetComponent<Interactable>().ShowIndicator();
			toInteract = obj;
		}
	}

	public void TakeDamage(int dmg, bool right)
	{
		anim.SetTrigger("Hit");
		active = false;
		hp -= dmg;
		flashTimer = 2f;
		gameObject.layer = 10;
		rb.velocity = new Vector2(right ? 10 : -10, 10);
		rb.gravityScale = grav;
		// TODO udpate hud
		if (hp <= 0)
		{
			Menu menu = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Menu>();
			aud.Play("die");
			menu.GameOver();
			return;
		}
		aud.Play("struck");
	}

	public void Heal(int health)
	{
		hp += health;
	  hp = hp > maxHP ? maxHP : hp;
		aud.Play("heal");
	}

	public void ActivateWeapon()
	{
		if(currentWeapon == "Sword")
		{
			sword.SetActive(true);
			aud.Play("sword");
		}
		else if(currentWeapon == "Staff")
		{
			staff.SetActive(true);
			aud.Play("staff");
		}
		if(Random.value > 0.25f)
			aud.Play("yell");
	}

	public void DeactivateWeapon()
	{
		attacking = false;
		sword.SetActive(false);
		staff.SetActive(false);
	}

	[YarnCommand("getsword")]
	public void AcquireSword()
	{
		hasSword = true;
		GameObject.Find("HUD").GetComponent<HUD>().ShowSword(false);
		anim.SetInteger("Weapon", 1);
	}

	[YarnCommand("getstaff")]
	public void AcquireStaff()
	{
		hasStaff = true;
		currentWeapon = "Staff";
		anim.SetInteger("Weapon", 2);
	}

}
