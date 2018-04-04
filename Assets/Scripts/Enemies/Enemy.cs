using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    //parameters set per type
    public string type;
    public float health;
    public Animator anim;
    public int damage;   //amount of damage done on contact
    public float speed;
    public float deathTime;

    //global objects/components
    public Player player;
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    // Flashing
    float flashTimer = 0;
    int flicker = 0;
    Color originalColor;

    //methods
    public virtual void Move() { }
    public virtual void Die() { }
    public virtual void Attack() { }
    public virtual void Update()
    {
      if(originalColor == null)
        originalColor = sr.color;
      if(health <= 0)
      {
          Die();
      }
      Move();
      Attack();
    }

  	public virtual void FixedUpdate()
  	{
  		if(flashTimer > 0)
  		{
  			flashTimer -= Time.deltaTime;
  			flicker++;
  			if(flicker%7 == 0)
          if(sr.color == originalColor)
				    sr.color = Color.white;
          else
            sr.color = originalColor;
  		}
  		else
  		{
  			if(sr.color != Color.white)
  				sr.color = Color.white;
  		}
  	}

    public virtual void TakeDamage(float dmg)
    {
        flashTimer = 0.5f;
        health -= dmg;
        GameObject.Find("Audio").GetComponent<AudioHandler>().Play("enemystruck");
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    public virtual void CollisionHandler(Collision2D col)
    {
      string tag = col.gameObject.tag;
      foreach (ContactPoint2D c in col.contacts)
      {
        if(c.collider.tag == "Player" || c.collider.tag == "Weapon")
        {
          tag = c.collider.tag;
          break;
        }
      }

      if(tag == "Player")
      {
        if (player.dashing)
        {
          TakeDamage(player.damage);
        }
        else
        {
          Vector3 dir = col.gameObject.transform.position - transform.position;
          player.TakeDamage(damage, dir.x > 0);
        }
      }
      else if(tag == "Weapon")
      {
        TakeDamage(player.damage);
      }
    }

    public virtual void TriggerHandler(Collider2D col, int dmg)
    {
        if (col.gameObject.tag == "Player")
        {
            if (player.dashing)
            {
                TakeDamage(player.damage);
            }
            else
            {
                Vector3 dir = col.gameObject.transform.position - transform.position;
                player.TakeDamage(dmg, dir.x > 0);
            }
        }
        else if (col.gameObject.tag == "Weapon")
        {
            TakeDamage(player.damage);
        }
    }
}
