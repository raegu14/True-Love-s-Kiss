using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    SpriteRenderer rend;
    public bool despawn = false;
    public float minBound = 1f;
    public float slope = 2;
    float speed = 1f;
    float fadeSpeed = 0.1f;
    Vector3 dir;
    bool gravity = false;

    bool blink;
    float blinkOffset;
    float nextBlink;
    float blinkIn;
    float prevAlpha;
    bool clamped;
    Vector2 bounds;

    // Pause menu things
    float lastTime = 0f;

    // Use this for initialization
    void Awake()
    {
        rend = this.GetComponent<SpriteRenderer>();
        blinkOffset = 2f;
        nextBlink = Random.Range(0f, 3f);
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            float t = lastTime;
            lastTime = Time.realtimeSinceStartup;
            float deltaTime = lastTime - t;
            if (deltaTime > 0.2f)
                return;
            rend.color -= new Color(0f, 0f, 0f, Random.value * fadeSpeed * deltaTime);
            transform.position -= dir * deltaTime * speed;
            if (rend.color.a <= 0f)
                Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        if(clamped)
        {
            if(transform.position.x > bounds.x || transform.position.y > bounds.y)
            {
                Destroy(gameObject);
            }
        }
        if (blink && Time.time > nextBlink)
        {
            Color col = rend.color;
            prevAlpha = col.a;
            col.a = 0;
            rend.color = col;
            nextBlink = Time.time + blinkOffset;
            blinkIn = Time.time + 1f;
        }
        else if(rend.color.a == 0)
        {
            if (Time.time > blinkIn)
            {
                Color col = rend.color;
                col.a = prevAlpha;
                rend.color = col;
            }
        }
        else if (despawn)
        {
            rend.color -= new Color(0f, 0f, 0f, Random.value * fadeSpeed * Time.deltaTime);
            if (!gravity)
                transform.position += dir * Time.deltaTime * speed;
            else
                GetComponent<Rigidbody2D>().AddForce(dir);
            if (rend.color.a <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }
    // Generic initialization
    public void Init(Vector3 basePosition, float color, float size, float bound, bool dirMatters)
    {
        bound = bound < minBound ? bound : minBound;
        despawn = true;
        Vector2 offset = Random.insideUnitCircle * bound;
        transform.position = basePosition - new Vector3(offset.x, offset.y, 0);
        rend.color = new Color(color, color, color, Random.value * 0.5f);
        float radius = 0.5f * Random.value;
        transform.localScale = new Vector3(size * radius, size * radius);

        float a, b;
        while (dirMatters)
        {
            a = Random.value;
            b = Random.value;
            if (b / a > slope)
            {
                if (Random.value > 0.5f)
                    dir = new Vector3(a, b, 0);
                else
                    dir = new Vector3(-1 * a, b, 0);
                dir.Normalize();
                return;
            }
        }

        dir = transform.position - basePosition;
        dir.Normalize();
        dir.x = Mathf.Clamp(dir.x, 0f, 0.5f);
    }

    // Init with bounds based on object size
    public void Init(GameObject obj, Vector3 basePosition, float color, float size)
    {
        SpriteRenderer r = obj.GetComponent<SpriteRenderer>();
        float bounds = r.bounds.size.x < minBound ? r.bounds.size.x : minBound;
        Init(basePosition, color, size, bounds, true);
    }

    // Fire and steam
    public void Init(GameObject obj, float size, float fadeMultiplier, float moveSpeed)
    {
        Init(obj, obj.transform.position, 1f, size);
        Set(fadeMultiplier, moveSpeed, false);
    }

    //specific direction
    public void Init(Vector3 basePosition, float color, float size, float bound, Vector3 direction, bool b, bool c, Vector2 max)
    {
        bound = bound < minBound ? bound : minBound;
        despawn = true;
        Vector2 offset = Random.insideUnitCircle * bound;
        transform.position = basePosition - new Vector3(offset.x, offset.y, 0);
        rend.color = new Color(color, color, color, Random.value * 0.5f);
        float radius = 0.5f * Random.value;
        transform.localScale = new Vector3(size * radius, size * radius);

        dir = direction.normalized;
        blink = b;
        clamped = c;
        bounds = max;
    }

    public void Set(float fadeMultiplier, float moveSpeed, bool grav)
    {
        fadeSpeed *= fadeMultiplier;
        speed = moveSpeed;
        if (grav)
        {
            gravity = grav;
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.mass = 0.5f; // TODO adjust gravity's effects
            rb.AddForce(new Vector2(dir.x, dir.y) * 50f);
        }
    }

    // Set bound to the 1.5 the width of the object
    public void SetBound(Vector3 bounds)
    {
        minBound = bounds.x;
    }

    public void SetSlope(float s)
    {
        slope = s;
    }
}
