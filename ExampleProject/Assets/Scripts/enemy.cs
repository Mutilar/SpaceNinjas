using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour 
{
	/* Reference to the player's location and components */
	public GameObject player;

	/* If the AI can see the player and move towards it */
	public bool moving;

	/*How far out the AI will recognize the player */
	public float sightDistance;


	/* Sets the duration of time between attacks */
	int attackCooldown = 50;
	/* Counts down from "attackCooldown", making a delay between attacks */
	int attackCounter = 0;

	/* Keeps track of which way you are looking */
	public bool goingLeft;


	/* How many hits they can take */
	public int life = 2;

	/* Sound effects to be played */
	public AudioClip swordSoundEffect;
	public AudioClip damageSoundEffect;

	//Fixed Update called every fixed framerate frame
	void FixedUpdate()
	{
		//Turns the AI white (resets from any red change from damage taken
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255);

		//If the AI has seen the player, move towards him
		if (moving)
		{
			//How fast the AI can move
			float speed = .02f;

			//If the player (given some buffer (show with and without buffer)) is to the left of the AI
			if (player.transform.position.x + .2f < this.transform.position.x)
			{
				goingLeft = true;
				//Move left
				transform.Translate(Vector3.left * speed);
				//Flip the AI to look left
				transform.localScale = new Vector2(1f,1f);
				//Set moving animation
				this.GetComponent<Animator>().SetBool ("moving", true);
			}
			//If the player (given some buffer) is to the right of the AI
			else if (player.transform.position.x - .2f > this.transform.position.x)
			{
				goingLeft = false;
				//Move left
				transform.Translate(Vector3.right * speed);
				//Flip the AI to look left
				transform.localScale = new Vector2(-1f,1f);
				//Set moving animation
				this.GetComponent<Animator>().SetBool ("moving", true);
			}
			//If the AI is close enough to the player to hit him
			else
			{
				//Assume the player has stopped moving
				this.GetComponent<Animator>().SetBool ("moving", false);

				//If the cooldown for attacking is done
				if (attackCounter == 0)
				{
					//Plays animation (overrides other animations)
					this.GetComponent<Animator>().Play("enemySwing");
					
					//Reset delay on attacking so you can't spam it
					attackCounter = attackCooldown;

					//Plays a soundfile at the player's position
					AudioSource.PlayClipAtPoint(swordSoundEffect, this.transform.position);


					//How far the raycast goes out (how far the sword reaches)
					float swingRange = .25f;
					
					//Variable that holds information on the Raycasts that will be performed
					RaycastHit2D swingInfo;

					if (goingLeft)
					{
						//Raycasts out from the left side of the player going left
						swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x - .1f, this.transform.position.y),new Vector2(0,-1f),swingRange);
					}
					else
					{
						//Raycasts out from the right side of the player going right
						swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x + .1f, this.transform.position.y),new Vector2(0,1f),swingRange);
					}
					//If the collider found something
					if (swingInfo.collider != null)
					{
						//If the swinging hit an enemy
						if (swingInfo.collider.gameObject.tag == "Player")
						{
							//Do damage to the enemy and run its animations
							swingInfo.collider.gameObject.GetComponent<player>().takeDamage();	
						}
					}				
				}
			}
		}
		//Else if the AI has not seen the player yet
		else
		{
			//If the distance between the enemy and the player is within the sight distance, move
			if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < sightDistance)
			{
				moving = true;
			}
		}

		//Delay (takes x seconds to get to 50 from 0) to make attacks spaced out
		if (attackCounter > 0)
		{
			attackCounter--;
		}

		//If the player falls off the islands, reset the level
		if (this.transform.position.y < -1f)
		{
			Destroy (this.gameObject);
		}
	}

	//Take Damage called whenever the AI takes a hit of any sort
	public void takeDamage()
	{
		//Turns the AI red
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0);

		//Plays a soundfile at the position of the AI
		AudioSource.PlayClipAtPoint(damageSoundEffect, this.transform.position);

		//Reduce the AI's life by 1
		life -= 1;

		//If the AI has no more life, destroy it
		if (life == 0)
		{
			Destroy (this.gameObject);
		}
	}

	//Trigger Enter called every time an object with a trigger collider contacts your collider
	void OnTriggerEnter2D(Collider2D other)
	{    
		//If collision is with the throwing star TODO::(change tag of throwing star to "star")
		if (other.gameObject.tag == "friend") 
		{
			//Destroy the star
			Destroy (other.gameObject);

			//Update the life of the AI and play sounds/animations
			takeDamage();
		}
	}
}
