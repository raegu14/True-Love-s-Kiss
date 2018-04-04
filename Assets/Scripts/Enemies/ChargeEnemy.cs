using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy {

    private Vector3 direction;         //left is true, right is false
    private bool charge;            //set charge animation once
    private float chargeDist;       //distance from player to charge
    private Vector3 rayDirection;   //direction to do raycast on collision
    private float attackCoolDown;
    private float attackRange;
    private float nextAttack;

    private float nextUpdate;
    private float updateTime;

    public GameObject weapon;
    private int frameCounter;
    private int attackFrame = 70;
    private bool attacking;
    private float totalIterations = 10;
    private float currIterations;
    private float minRotation;
    private float maxRotation;
    private int weaponDamage = 2;

    // Use this for initialization
    void Start()
    {
        //enemy parameters
        type = "charge";
        health = 3;
        anim = GetComponent<Animator>();
        damage = 1;
        speed = 5f;
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        deathTime = 0.1f;
        //chargeEnemy parameters
        rayDirection = Vector2.right;
        chargeDist = 40f;
        attackCoolDown = 4f;
        attackRange = 15f;
        minRotation = 40f;
        maxRotation = 200f;
        updateTime = 0.1f;
    }

    // Update is called once per frame

    public override void Move()
    {
        /*
        if (Time.time > nextUpdate)
        {
            nextUpdate = Time.time + updateTime;
            DestroyImmediate(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
        } */

        anim.SetBool("Walking", false);
        if (attacking)
        {
            weapon.SetActive(true);
            currIterations++;
            weapon.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(minRotation, maxRotation, currIterations / totalIterations));
            if (currIterations == totalIterations)
            {
                attacking = false;
                currIterations = 0;
                StartCoroutine(disableWeapon());
            }
        }

        direction = player.gameObject.transform.position - transform.position;
        if (direction.magnitude < chargeDist)
        {
            anim.SetBool("Walking", true);
            if (!charge)
            {
                charge = true;
                //play charge animation
            }
            //left case
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BigAttacking"))
            {
                if (direction.x < 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = 5;
                    transform.localScale = scale;
                    rb.MovePosition(rb.position + new Vector2(-Time.deltaTime * speed, 0));
                }
                else
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -5;
                    transform.localScale = scale;
                    rb.MovePosition(rb.position + new Vector2(Time.deltaTime * speed, 0));
                }
            }
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("BigAttacking") && direction.magnitude < attackRange && Time.time > nextAttack)
        {
            anim.SetTrigger("Attack");
            nextAttack = Time.time + attackCoolDown;
            StartCoroutine(attack());
        }

    }

    IEnumerator attack()
    {
        /*
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        } */
        yield return new WaitForSeconds(0.95f);
        attacking = true;
    }

    IEnumerator disableWeapon()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }
        weapon.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        {
            /*
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, rayDirection, 5f, LayerMask.GetMask("Ground"));
            if (Mathf.Abs(hit1.normal.x) == 1f)
            {
                // bounce back and stunned
                stunTime = Time.time + chargeCoolDown;
            } */
            Physics2D.IgnoreCollision(col.rigidbody.GetComponent<Collider2D>(), weapon.transform.GetComponent<Collider2D>());
        }
        CollisionHandler(col);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        TriggerHandler(col, weaponDamage);
    }

    public override void Die()
    {
        //play animation
        //invoke death
        Invoke("Death", deathTime);
    }
}
