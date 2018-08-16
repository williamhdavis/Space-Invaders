using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
	private bool fired;
	private float offset = 0.5f;
	private Rigidbody2D rb2d;
	private float movementDistance = 0.5f;

	// Use this for initialization
	public void Start ()
	{
		this.fired = false;
		this.rb2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if (fired)
		{
			this.rb2d.transform.position = (Vector2)this.rb2d.transform.position + new Vector2 (0, this.movementDistance);
		}
	}

	public void fire(Vector2 position)
	{
		if (!this.fired)
		{
			this.fired = true;
			this.rb2d.transform.position = position + new Vector2 (0, offset);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "TopTrigger" || other.tag == "Alien")
		{
			this.rb2d.transform.position = GameController.instance.poolPosition;
			this.fired = false;
		}
	}
}
