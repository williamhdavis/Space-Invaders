using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
	private int alienType;
	private Vector2 spawn;
	private float movementDistance = 0.5f;
	private Rigidbody2D rb2d;
	private Animator anim;
	private bool dead;
	private bool shouldShoot;
	private bool pooled;
	private float timeSinceDeath;
	private float vanishRate = 0.25f;

	public void setAlienType(int alienType)
	{
		if (alienType > 4)
		{
			alienType = 4;
		}
		this.alienType = alienType;
		this.anim.SetInteger ("Type", this.alienType);
	}

	// Used to set start point.
	public void setAlienSpawn(Vector2 spawn)
	{
		this.spawn = spawn;
	}

	public void Awake()
	{
		this.alienType = 0;
		this.anim = GetComponent<Animator> ();
		this.anim.SetInteger ("Type", this.alienType);
	}

	// Use this for initialization
	public void Start ()
	{
		this.rb2d = GetComponent<Rigidbody2D> ();
		this.dead = false;
		this.pooled = false;
		this.detectAliens();
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(!this.dead)
		{
			if (this.shouldShoot)
			{
				//Shoot code.
			}
			else
			{
				this.detectAliens();
			}
		}
		else if(!this.pooled)
		{
			this.timeSinceDeath += Time.deltaTime;
			if(this.timeSinceDeath > this.vanishRate)
			{
				this.rb2d.transform.position = GameController.instance.poolPosition;
				this.pooled = true;
			}
		}
	}

	public void moveLeft()
	{
		this.move(-1);
	}

	public void moveRight()
	{
		this.move(1);
	}

	private void move(int direction)
	{
		if(!this.dead)
		{
			this.rb2d.transform.position = (Vector2)this.rb2d.transform.position + new Vector2 (this.movementDistance * direction, 0);
			this.anim.SetBool ("Moving", !anim.GetBool ("Moving"));
		}
	}

	public void drop()
	{
		if(!this.dead)
		{
			this.rb2d.transform.position = (Vector2)this.rb2d.transform.position + new Vector2 (0, this.movementDistance * -1);
			this.anim.SetBool ("Moving", !anim.GetBool ("Moving"));
		}
	}

	public bool isDead()
	{
		return this.dead;
	}

	public bool isPooled()
	{
		return this.pooled;
	}

	private int getScore()
	{
		int score;
		switch(this.alienType)
		{
			case 1: score = 10; break;
			case 2: score = 20; break;
			case 3: score = 30; break;
			case 4: score = 200; break;
			default: score = 0; break;
		}
		return score;
	}

	private void detectAliens()
	{
		float length = (this.rb2d.transform.position.y - Player.instance.transform.position.y) - 1;
		RaycastHit2D hit = Physics2D.Raycast ((Vector2)this.transform.position + new Vector2(0, -0.82f), Vector2.down, length);
		if(hit.collider != null)
		{
			if (hit.collider.tag == "Alien") 
			{
				this.shouldShoot = false;
				GetComponent<SpriteRenderer> ().color = Color.red;
				return;
			}
		}
		this.shouldShoot = true;
		GetComponent<SpriteRenderer> ().color = Color.green;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "GameOver" || other.tag == "Player")
		{
			GameController.instance.setGameOver();
		}
		else if(other.tag == "LeftTrigger" || other.tag == "RightTrigger")
		{
			GameController.instance.reverseAliens ();
		}
		else if (other.tag == "PlayerBullet")
		{
			this.dead = true;
			this.anim.SetBool ("Dead", true);
			this.timeSinceDeath = 0;
			GameController.instance.addScore(this.getScore());
		}
	}
}
