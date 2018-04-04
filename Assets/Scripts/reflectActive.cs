using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reflectActive : MonoBehaviour {

    public GameObject reflectWall;

    private bool active;
    private float startTime;
    private float activeTime;

    private Animator anim;

    private float totalFade;
    private float currentFade;
    private Color noColor;
    private Color fullColor;
    private SpriteRenderer sr;
    private bool fadeIn;

    private Menu menu;

    // Use this for initialization
    void Start () {
        activeTime = 0.5f;
        anim = GetComponent<Animator>();
        sr = reflectWall.GetComponent<SpriteRenderer>();
        fullColor = sr.color;
        noColor = sr.color;
        noColor.a = 0;
        totalFade = 15;
        menu = GameObject.Find("Main Camera").GetComponent<Menu>();
	}

	// Update is called once per frame
	void Update () {
        reflectWall.transform.position = transform.position + (transform.localScale.x) * new Vector3(10f, 0, 0);
        if(!menu.paused)
        {
            if (Input.GetAxisRaw("Reflect") > 0 && !active && GetComponent<Animator>().GetBool("Ground"))
            {
                anim.SetTrigger("Reflect");
                active = true;
                reflectWall.SetActive(active);
                gameObject.GetComponent<Player>().SetActive(false);
                startTime = Time.time;
                currentFade = 1;
                fadeIn = true;
    						GameObject.Find("Audio").GetComponent<AudioHandler>().Play("reflect");
    						Debug.Log("projectile");
            }

            if (active && Time.time > startTime + activeTime) {
                  active = false;
                  reflectWall.SetActive(active);
                  gameObject.GetComponent<Player>().SetActive(true);
            }
            if (currentFade > 0)
            {
                sr.color = Color.Lerp(noColor, fullColor, currentFade / totalFade);
                if (fadeIn) {
                    currentFade *= 1.2f;
                    if(currentFade > totalFade)
                    {
                        fadeIn = false;
                    }
                }
                else
                {
                    currentFade /= 1.1f;
                }
            }

        }
    }
}
