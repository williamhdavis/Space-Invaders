using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static GameController instance;
	public readonly Vector2 poolPosition = new Vector2 (-15, -25);
	public Text scoreText;
	public GameObject gameOverScreen;
	public Text endText;
	public Alien alienPrefab;

	private bool gameOver;
	private bool goingRight;
	private bool reversing;
	private bool dropped;
	private float timeSinceLastMovement;
	private float movementRate = 0.5f;
	private Alien[][] aliens;
	private int alienRows;
	private int alienColumns;
	private int alienTypeChange;
	private float alienWidth;
	private float alienHeight;
	private float alienXSeperation;
	private float alienYSeperation;
	private int score;

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
		this.gameOver = false;
		this.gameOverScreen.SetActive(false);
		this.goingRight = true;
		this.reversing = false;
		this.dropped = false;
		this.timeSinceLastMovement = 0;
		this.score = 0;

		this.alienRows = 5;
		this.alienColumns = 11;
		this.alienTypeChange = 2;
		this.alienWidth = 1.22f;
		this.alienHeight = 0.82f;
		this.alienXSeperation = 0.72f;
		this.alienYSeperation = 0.52f;

		float totalWidth = (this.alienWidth + this.alienXSeperation) * (this.alienColumns - 1);
		float totalHeight = (this.alienHeight + this.alienYSeperation) * (this.alienRows - 1);

		Vector2 spawnStart = new Vector2 ((totalWidth / 2f) * -1, (totalHeight / 2f) * -1);
		float xChange = this.alienWidth + this.alienXSeperation;
		float yChange = this.alienHeight + this.alienYSeperation;

		this.aliens = new Alien[this.alienRows][];

		int alienType = 1;
		int y = 0;
		while(y < this.alienRows)
		{
			if(y % this.alienTypeChange == 0 && y != 0)
			{
				alienType++;
			}
			this.aliens[y] = new Alien[this.alienColumns];
			int x = 0;
			while(x < this.alienColumns)
			{
				Vector2 pos = spawnStart + new Vector2(xChange * x, yChange * y);
				this.aliens[y][x] = (Alien)Instantiate (this.alienPrefab, pos, Quaternion.identity);
				this.aliens[y][x].setAlienType(alienType);
				this.aliens[y][x].setAlienSpawn(pos);
				++x;
			}
			++y;
		}
	}

	// Update is called once per frame
	public void Update ()
	{
		if (!this.gameOver)
		{
			this.timeSinceLastMovement += Time.deltaTime;
			if (this.timeSinceLastMovement >= this.movementRate)
			{
				//Move aiens
				this.timeSinceLastMovement = 0;
				bool alienAlive = false;

				foreach(Alien[] alienRow in this.aliens)
				{
					foreach(Alien alien in alienRow)
					{
						if(!alien.isPooled() && !alien.isDead())
						{
							if(this.reversing && !this.dropped)
							{
								alien.drop();
							}
							else if(this.goingRight)
							{
								alien.moveRight();
							}
							else
							{
								alien.moveLeft();
							}
							alienAlive = true;
						}
					}
				}
				
				if(!alienAlive)
				{
					this.setGameWin();
				}

				if(this.reversing && !this.dropped)
				{
					this.dropped = true;
					this.goingRight = !this.goingRight;
				}
				else if(this.reversing)
				{
					this.reversing = false;
					this.dropped = false;
				}
			}
		}
		else
		{
			// Reset the game
			if(Input.GetButton("Fire1"))
			{
				
			}
		}
	}

	private void setGameWin()
	{
		this.gameOver = true;
		this.gameOverScreen.SetActive(true);
		this.endText.text = "VICTORY";
	}

	public void setGameOver()
	{
		this.gameOver = true;
		this.gameOverScreen.SetActive(true);
		this.endText.text = "GAME OVER";
	}

	public bool isGameOver()
	{
		return this.gameOver;
	}
	public void reverseAliens()
	{
		if(!this.reversing)
		{
			this.reversing = true;
		}
	}

	public void addScore(int score)
	{
		this.score += score;
		this.scoreText.text = "<Score>\n" + string.Format("{0:0000000}", this.score);
	}
}
