using UnityEngine;
using System.Collections;

public class enemyRanged : MonoBehaviour {

	/* Reference to the player's location and components */
	public GameObject player;
	
	/* If the AI can see the player and move towards it */
	public bool moving;
	
	/*How far out the AI will recognize the player */
	public float sightDistance;

	/* Prefab of a laser */
	public GameObject laser;

	/* Sets the duration of time between attacks */
	int attackCooldown = 50;
	/* Counts down from "attackCooldown", making a delay between attacks */
	int attackCounter = 0;
	
	/* Keeps track of which way you are looking */
	public bool goingLeft;
	
	
	/* How many hits they can take */
	public int life = 2;
	
	/* Sound effects to be played */
	public AudioClip laserSoundEffect;
	public AudioClip damageSoundEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate()
	{
		//Turns the AI white (resets from any red change from damage taken
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255);

		//If the AI has seen the player, move towards him
		if (moving)
		{			
			//How fast the AI can move
			float speed = .01f;
			
			//If the player (given some buffer (show with and without buffer)) is to the left of the AI
			if (player.transform.position.x + .8f < this.transform.position.x)
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
			else if (player.transform.position.x - .8f > this.transform.position.x)
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
					this.GetComponent<Animator>().Play("enemyShoot");
					
					//Reset delay on attacking so you can't spam it
					attackCounter = attackCooldown;
					
					//Makes a new throwing star at the player's position
					GameObject laserX =  Instantiate(laser, this.transform.position,this.transform.rotation) as GameObject;
					
					//Sets which way the star is going depending on which way the player is looking
					laserX.GetComponent<starPhysics>().goingLeft = goingLeft;

					//Plays a soundfile at the AI's position
					AudioSource.PlayClipAtPoint(laserSoundEffect, this.transform.position);
									
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
		if (attackCounter > 0) attackCounter--;
		
		//If the player falls off the islands, reset the level
		if (this.transform.position.y < -1f)
			Destroy (this.gameObject);
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
		//If collision is with the throwing star
		if (other.gameObject.tag == "friend") 
		{
			//Destroy the star
			Destroy (other.gameObject);
			
			//Update the life of the AI and play sounds/animations
			takeDamage();
		}
	}




}
