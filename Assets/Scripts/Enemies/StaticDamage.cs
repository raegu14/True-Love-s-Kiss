using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDamage : Enemy
{

    private float cooldown;
    private float attackTime;

    private bool particleSpawn;
    private float PARTICLE_PROBABILITY;
    public GameObject particles;
    private int numParticles;

    private Color fullColor;
    private Color noColor;
    private float step;

    // Use this for initialization
    void Start()
    {
        //enemy parameters
        type = "staticDamage";
        health = Mathf.Infinity;
        anim = GetComponent<Animator>();
        damage = 1;
        player = GameObject.Find("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        deathTime = 0.2f;

        //staticEnemy parameters
        cooldown = 1f;
        attackTime = Time.time;
        PARTICLE_PROBABILITY = 0.8f;
        numParticles = 5;
        fullColor = sr.color;
        noColor = sr.color;
        noColor.a = 0;
    }

    public override void Move()
    {
        if (particleSpawn)
        {
            sr.color = Color.Lerp(fullColor, noColor, step / deathTime);
            step += Time.deltaTime;
            for (int i = 0; i < numParticles; i++)
            {
                //&& Random.value < PARTICLE_PROBABILITY)
                {
                    GameObject temp = Instantiate(particles);
                    temp.GetComponent<Particles>().SetBound(sr.bounds.extents);
                    temp.GetComponent<Particles>().Init(transform.position, 1f, 1f, 2f, true);
                }
            }
        }
    }

    // Update is called once per frame
    void OnTriggerStay2D(Collider2D col)
    {
        // TODO
        /*
        string tag = col.gameObject.tag;
        foreach (ContactPoint2D c in col.contacts)
        {
          if(c.gameObject.tag == "Player" || c.gameObject.tag == "Weapon")
          {
            tag = c.collider.tag;
            break;
          }
        }
        */
        if (col.gameObject.tag == "Weapon")
        {
            if (player.currentWeapon == "Sword" && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordAttack"))
            {
                particleSpawn = true;
                Invoke("Death", deathTime);
            }
        }

        if (col.gameObject.tag == "Player")
        {
            if(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordDash"))
            {
                particleSpawn = true;
                Invoke("Death", deathTime);
            }
            else if(attackTime < Time.time)
            {
                attackTime = Time.time + cooldown;
                Vector3 dir = col.gameObject.transform.position - transform.position;
                player.TakeDamage(damage, dir.x > 0);
            }
        }
    }
}
