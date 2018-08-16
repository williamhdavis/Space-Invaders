using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;
	public PlayerBullet bulletPrefab;

	private float movementDistance = 0.1f;
	private Rigidbody2D rb2d;
	private bool stopMoveLeft;
	private bool stopMoveRight;
	private int lives = 3;
	private PlayerBullet bullet;

	public void Awake ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	public void Start ()
	{
		this.rb2d = GetComponent<Rigidbody2D> ();
		this.stopMoveLeft = false;
		this.stopMoveRight = false;
		this.bullet = (PlayerBullet)Instantiate (bulletPrefab, GameController.instance.poolPosition, Quaternion.identity);
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(!GameController.instance.isGameOver())
		{
			if(Input.GetButton("Fire1"))
			{
				this.bullet.fire (this.rb2d.transform.position);
			}
			float input = Input.GetAxis ("Horizontal");
			if (input < 0 && !stopMoveLeft)
			{
				this.rb2d.transform.position = (Vector2)this.rb2d.transform.position + new Vector2 (movementDistance * -1, 0);
				this.stopMoveRight = false;
			}
			else if (input > 0 && !stopMoveRight)
			{
				this.rb2d.transform.position = (Vector2)rb2d.transform.position + new Vector2 (movementDistance, 0);
				this.stopMoveLeft = false;
			}
		}
	}

	public int getPlayerLives()
	{
		return this.lives;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "LeftTrigger")
		{
			this.stopMoveLeft = true;
		}
		else if(other.tag == "RightTrigger")
		{
			this.stopMoveRight = true;
		}
	}
}
