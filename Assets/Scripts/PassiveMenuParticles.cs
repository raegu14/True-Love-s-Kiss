using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveMenuParticles : MonoBehaviour {

    private float PARTICLE_PROBABILITY;

    public GameObject particles;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

	// Use this for initialization
	void Start () {
        PARTICLE_PROBABILITY = 0.11f;
        minX = -10f;
        minY = -5f;
        maxX = 10f;
        maxY = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < PARTICLE_PROBABILITY/2)
        {
            GameObject temp = Instantiate(particles);
            Vector3 pos = new Vector3(minX, Random.Range(minY, maxY),0f);
            temp.GetComponent<Particles>().Set(0.5f, 2, false);
            temp.GetComponent<Particles>().Init(pos, 1f, 0.25f, 1f, new Vector3(2, -1, 0), true, true, new Vector2(maxX, maxY));
        }

        if (Random.value < PARTICLE_PROBABILITY)
        {
            GameObject temp = Instantiate(particles);
            Vector3 pos = new Vector3(Random.Range(minX, maxX), maxY, 0f);
            temp.GetComponent<Particles>().Set(0.5f, 2, false);
            temp.GetComponent<Particles>().Init(pos, 1f, 0.25f, 1f, new Vector3(2, -1, 0), true, true, new Vector2(maxX, maxY));
        }
    }
}
