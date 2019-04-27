//******************************************************//
//														//
// 				WRITTEN BY BRIAN HUNGERMAN 				//
// FOR THE USE OF MONTE VISTA'S SUMMER OF GAME DESIGN	//
//														//
//******************************************************//

using UnityEngine;
/* UI needed to handle the "Image" component from GUI elements */
using UnityEngine.UI; 		
using System.Collections;

public class player : MonoBehaviour 
{

	/* Keeps track of which way you are looking */
	public bool goingLeft;

	/* Sets the duration of time between attacks */
	int attackCooldown = 50;
	/* Counts down from "attackCooldown", making a delay between attacks */
	int attackCounter = 0;
	
	/* How many hits you can take */
	int life = 3;
	/* GUI display for each life point */
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;

	/* Prefab of a throwing star */
	public GameObject star;

	/* How many stars you can throw */
	 int ammo = 3;
	/* GUI display for each star */
	public GameObject ammo1;
	public GameObject ammo2;
	public GameObject ammo3;

	/* GUI display for wininng */
	public GameObject display;

	/* Sound effects to be played */
	public AudioClip starSoundEffect;
	public AudioClip swordSoundEffect;
	public AudioClip damageSoundEffect;

	//Update called every in-game frame
	void Update()
	{
		//How fast the player is moving (this can be changed)
		float speed = .02f;
		
		//Assume the player has stopped moving
		this.GetComponent<Animator>().SetBool ("moving", false);
		
		//Resets the player's color to white
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255);
		
		//Check if the player is actually moving (A and D as Left and Right)
		if (Input.GetKey(KeyCode.A))
		{
			//Keeps track of which way the player is looking for other functions
			goingLeft = true;
			
			//Move the player left at the rate of the speed variable
			transform.Translate(Vector3.left * speed);
			
			//Make sure the player is looking left
			transform.localScale = new Vector2(1f,1f);
			
			//Plays animation for walking 
			this.GetComponent<Animator>().SetBool ("moving", true);
		}
		if (Input.GetKey(KeyCode.D))
		{
			//Keeps track of which way the player is looking for other functions
			goingLeft = false;
			
			//Move the player right at the rate of the speed variable
			transform.Translate(Vector3.right * speed);
			
			//Make sure the player is looking right
			transform.localScale = new Vector2(-1f,1f);
			
			//Plays animation for walking
			this.GetComponent<Animator>().SetBool ("moving", true);
		}
		
		//Delay (takes x seconds to get to 50 from 0) to make attacks spaced out
		if (attackCounter > 0) attackCounter--;
		
		//If the player falls off the islands, reset the level
		if (this.transform.position.y < -1f)
			die ();

		//How far the raycast goes down (to see if there is ground below the player)
		float detectionRange = .1f;

		//Raycasts out from the bottom of the player going down
		RaycastHit2D hitInfo = Physics2D.Raycast(  new Vector2(this.transform.position.x, this.transform.position.y - .1f),new Vector2(0,-1f),detectionRange);			
	
		//If there is ground below the player (assume no at first)
		bool grounded = false;

		//If the raycast hits something below the player, it is "grounded" and can jump again
		if (hitInfo.collider != null)
		{
			grounded = true;	
		}

		//Pressing down "W" and if there is ground below the player+
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (grounded == true)
			{
				//AddForce forces the player upwards by "165" units of power
				this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,165f));
			}
		}
		//Left-Click
		if (Input.GetMouseButtonDown(0))
		{
			//If the cooldown for attacking is done
			if (attackCounter == 0)
			{
				//Plays animation (overrides other animations)
				this.GetComponent<Animator>().Play("swing");

				//Reset delay on attacking so you can't spam it
				attackCounter = attackCooldown;

				//Plays a soundfile at the player's position
				AudioSource.PlayClipAtPoint(swordSoundEffect, this.transform.position);

				//How far the raycast goes out (how far the sword reaches)
				float swingRange = .25f;

				//Variable that holds information on the Raycasts that will be performed
				RaycastHit2D swingInfo;

				if (goingLeft == true)
				{
					//Raycasts out from the left side of the player going left
					swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x - .1f, this.transform.position.y),new Vector2(-1f,0f),swingRange);
				}
				else
				{
					//Raycasts out from the right side of the player going right
					swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x + .1f, this.transform.position.y),new Vector2(1f,0f),swingRange);
				}
				//If the collider found something
				if (swingInfo.collider != null)
				{
					//If the swinging hit an enemy
					if (swingInfo.collider.gameObject.tag == "enemy")
					{
						//Do damage to the enemy and run its animations
						swingInfo.collider.gameObject.GetComponent<enemy>().takeDamage();	
					}
					if (swingInfo.collider.gameObject.tag == "enemyRanged")
					{
						//Do damage to the enemy and run its animations
						swingInfo.collider.gameObject.GetComponent<enemyRanged>().takeDamage();	
					}
				}				
			}
		}		
		//Right-click
		if (Input.GetMouseButtonDown(1))
		{
			//If the cooldown for attacking is done
			if (attackCounter == 0)
			{
				//If the player has enough stars to throw
				if (ammo > 0)
				{
					//Plays animation (overrides other animations)
					this.GetComponent<Animator>().Play("throw");

					//Reset delay on attacking so you can't spam it
					attackCounter = attackCooldown;

					//Makes a new throwing star at the player's position
					GameObject starX =  Instantiate(star, this.transform.position,this.transform.rotation) as GameObject;

					//Sets which way the star is going depending on which way the player is looking
					starX.GetComponent<starPhysics>().goingLeft = goingLeft;

					//Plays a soundfile at the player's position
					AudioSource.PlayClipAtPoint(starSoundEffect, this.transform.position);

					//Use up one star's worth of ammo
					ammo -= 1;

					//Updates stars on the GUI
					if (ammo == 2)
					{
						ammo3.SetActive(false);
					}
					if (ammo == 1) 
					{
						ammo2.SetActive(false);
					}
					if (ammo == 0)
					{
						ammo1.SetActive(false);
					}
				}
			}
		}
	}


	//Trigger Enter called every time an object with a trigger collider contacts your collider
	void OnTriggerEnter2D(Collider2D other)
	{    
		//If the thing you touched is "tagged" as an enemy
		if (other.gameObject.tag == "enemy") 
		{
			//Update hearts on the GUI
			takeDamage ();
		}
		//If the player enters the Dojo, display completion text
		if (other.gameObject.tag == "Finish") 
		{
			display.SetActive(true);
		}
	}

	//Take Damage called whenever the AI takes a hit of any sort
	public void takeDamage()
	{ 
		//Turns the player red
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0);

		//Plays a soundfile at the player's position
		AudioSource.PlayClipAtPoint(damageSoundEffect, this.transform.position);

		//Takes damage, one point of life
		life -= 1;

		//Updates the hearts on the GUI
		if (life == 2) 
		{
			heart3.SetActive(false);
		}
		if (life == 1) 
		{
			heart2.SetActive(false);
		}
		if (life == 0) 
		{
			//If the player has no more life, the level is reseted
			die();
		}

	}


	//Die called when the player has no more life
	public void die()
	{
		//Simply resets the scene to play again
		Application.LoadLevel ("world");
	}

}
