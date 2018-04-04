using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Vector3 direction;          //left is true, right is false
    private bool reflected;         //reflected -> do damage
    private CapsuleCollider2D col;
    private Player player;
    private float speed;
    private int damage;

    public bool spawnCrystals;
    public GameObject crystal;
    private Vector3 yOffset;

    // Use this for initialization
    void Start () {
        col = GetComponent<CapsuleCollider2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        speed = 15f;
        damage = 1;
        yOffset = new Vector3(0, 4, 0);

	}

	// Update is called once per frame
	void Update () {
        transform.position += direction.normalized * Time.deltaTime * speed;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (reflected)
            {
                print(col.gameObject);
                col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(spawnCrystals && col.gameObject.name == "Crystalable")
            {
                Instantiate(crystal, col.gameObject.transform.position + yOffset, col.gameObject.transform.rotation);
                //spawn crystal on top of col
            }
            Destroy(this.gameObject);
        }
        else if(col.gameObject.tag == "Player")
        {
            if (!player.dashing)
            {
                Vector3 dir = col.gameObject.transform.position - transform.position;
                player.TakeDamage(damage, dir.x > 0);
                Destroy(this.gameObject);
            }
        }
        else if(col.gameObject.name == "Reflect")
        {
            if (!reflected)
            {
                direction = -direction;
                reflected = true;
            }
        }
    }
}
