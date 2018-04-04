using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy {

    private Vector3 direction;      //left is true, right is false
    private float fireCoolDown;     //time between shooting projectiles
    private float fireTime;         //next time to fire projectile
    private float fireRange;        //distance from player to fire

    //projectile stats
    public GameObject projectile;   //projectile script
    private Vector3 projectileOffset;
    public bool spawnCrystal;
    private float scaleX;

    private int numBossProjectiles;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 dir;
    private float pauseTime;
    private float nextMoveTime;
    private int firedProjectiles;
    private bool left;

    // Use this for initialization
    void Start()
    {
        //enemy parameters
        type = "projectile";
        health = 1;
        damage = 1;
        speed = 5f;
        deathTime = 0.1f;
        player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //projectileEnemy parameters
        fireCoolDown = 2f;
        fireRange = 25f;
        fireTime = 0;
        projectileOffset = new Vector3(3f, 0, 0);
        scaleX = transform.localScale.x;

        if (gameObject.name == "Projectile Boss")
        {
            fireCoolDown = 3f;
            left = true;
            health = 6;
            leftPos = new Vector3(7, 0, 0);
            rightPos = new Vector3(67, 0, 0);
            dir = Vector3.left;
            pauseTime = 5f;
            nextMoveTime = Time.time + pauseTime;
            speed = 20f;
            fireTime = 0;
            numBossProjectiles = 2;
        }

    }


    public override void FixedUpdate()
    {
    }

    public override void Move()
    {
        direction = player.gameObject.transform.position - transform.position;

        //change direction of sprite
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = scaleX;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -scaleX;
            transform.localScale = scale;
        }

        if(gameObject.name == "Projectile Boss")
        {
            if (Time.time < nextMoveTime && Time.time > fireTime)
            {
                anim.SetTrigger("Shoot");
                StartCoroutine(Shoot());
                fireTime = Time.time + fireCoolDown;
            }
            else if(Time.time > nextMoveTime)
            {
                transform.position += dir * Time.deltaTime * speed;
                if(left && transform.position.x < leftPos.x)
                {
                    left = false;
                    dir = -dir;
                    nextMoveTime = Time.time + pauseTime;
                }
                else if (!left && transform.position.x > rightPos.x)
                {
                    left = true;
                    dir = -dir;
                    nextMoveTime = Time.time + pauseTime;
                }
            }
        }

        //if in range
        else if (direction.magnitude < fireRange && Time.time > fireTime)
        {
            fireTime = Time.time + fireCoolDown;
            //left case
            anim.SetTrigger("Shoot");
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1.5f);
        if (direction.x < 0)
        {
            if (gameObject.name == "Projectile Boss")
            {
                for (int i = 0; i < numBossProjectiles; i++)
                {
                    GameObject proj = Instantiate(projectile, transform.position - projectileOffset, transform.rotation);
                    proj.GetComponent<Projectile>().direction = direction + (new Vector3(Random.Range(-5f, 5f), 0));
                    proj.GetComponent<Projectile>().spawnCrystals = spawnCrystal;
                }
            }
            else
            {
                GameObject proj = Instantiate(projectile, transform.position - projectileOffset, transform.rotation);
                proj.GetComponent<Projectile>().direction = direction;
                proj.GetComponent<Projectile>().spawnCrystals = spawnCrystal;
            }
        }
        else
        {
            if (gameObject.name == "Projectile Boss")
            {
                GameObject proj = Instantiate(projectile, transform.position + projectileOffset, transform.rotation);
                proj.GetComponent<Projectile>().direction = direction + (new Vector3(Random.Range(-5f, 5f), 0));
                proj.GetComponent<Projectile>().spawnCrystals = spawnCrystal;
            }
            else
            {
                GameObject proj = Instantiate(projectile, transform.position + projectileOffset, transform.rotation);
                proj.GetComponent<Projectile>().direction = direction;
                proj.GetComponent<Projectile>().spawnCrystals = spawnCrystal;
            }
        }
        GameObject.Find("Audio").GetComponent<AudioHandler>().Play("magic");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        CollisionHandler(col);
    }

    public override void Die()
    {
        //play animation
        //invoke death
        Invoke("Death", deathTime);
    }
}
