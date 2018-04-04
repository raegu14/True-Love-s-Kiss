using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWay : MonoBehaviour {

	public float TIMER = 0.5f;
	public float MARGIN = 0.25f;
	public float multiplier = 2;
	float dist;
	float timer;
	Collider2D thisCol;
	Vector2 size;
	Vector3 offset;
	Vector3 baseOffset;
	int layerMask;
	bool above = false;

	// Use this for initialization
	void Start () {
		thisCol = GetComponent<Collider2D>();
		size = GetComponent<SpriteRenderer>().bounds.size;
		layerMask = LayerMask.GetMask("Player", "Player(invuln)");
		dist = GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.y;
		offset = new Vector3(0, dist + GetComponent<SpriteRenderer>().bounds.extents.y, 0);
		baseOffset = offset;
		dist *= multiplier;
		timer = TIMER;
	}

	void Update()
	{
		if(timer > 0)
		{
			timer -= Time.deltaTime;
			return;
		}
		if(above)
			offset = baseOffset - new Vector3(0, MARGIN, 0);
		else
			offset = baseOffset;
		RaycastHit2D hit = Physics2D.BoxCast(transform.position + offset, size, 0,
			Vector2.up, dist, layerMask);

		if(hit.collider == null)
		{
			thisCol.isTrigger = true;
			above = false;
		}
		else
		{
			thisCol.isTrigger = false;
			above = true;
		}
	}

	public void TempDisable()
	{
		above = false;
		timer = TIMER;
		thisCol.isTrigger = true;
	}
}
