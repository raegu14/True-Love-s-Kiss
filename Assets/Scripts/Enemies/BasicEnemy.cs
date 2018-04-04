using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {

  private bool direction;     //left is true, right is false
  private Vector3 rayDirection;  //direction to do raycast on collision

  // Use this for initialization
  void Start()
  {
    SetUp();
  }

  public virtual void SetUp()
  {
    //enemy parameters
    type = "basic";
    health = 1;
    anim = GetComponent<Animator>();
    damage = 1;
    speed = 3;
    player = GameObject.Find("Player").GetComponent<Player>();
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    deathTime = 0.1f;
    //basicEnemy parameters
    direction = false;
    rayDirection = Vector2.right;
  }

  // Update is called once per frame

  public override void Move()
  {
    if (direction)
    {
      transform.position += new Vector3(-Time.deltaTime * speed, 0);
    }
    else
    {
      transform.position += new Vector3(Time.deltaTime * speed, 0);
    }
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if(col.gameObject.layer == 8)
    {
      RaycastHit2D hit1 = Physics2D.Raycast(transform.position, rayDirection, 5f, LayerMask.GetMask("Ground"));
      if (Mathf.Abs(hit1.normal.x) == 1f)
      {
        rayDirection = -rayDirection;
        direction = !direction;
        sr.flipX = !sr.flipX;
      }
    }
    CollisionHandler(col);
  }

  public override void Die()
  {
    //play animation
    //invoke death
    Invoke("Death", deathTime);
  }
}
