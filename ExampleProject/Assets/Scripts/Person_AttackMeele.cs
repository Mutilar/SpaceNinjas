using UnityEngine;
using System.Collections;

public class Person_AttackMeele : MonoBehaviour {

	
	//Sets the duration of time between attacks
	int attack_cooldown = 50;
	//Counts down from "attackCooldown", making a delay between attacks
	int attack_counter = 0; 

	public bool AI_enabled;

	public Transform AI_target;

	public float target_swingDistance;

	public bool SFX_enabled;

	public AudioClip SFX_sword;
	

	// Update is called once per frame
	void Update () 
	{
		//Delay (takes x seconds to get to 50 from 0) to make attacks spaced out
		if (attack_counter > 0) attack_counter--;

		//Left-Click
		if (AI_enabled == false && Input.GetMouseButtonDown(0))
		{
			attack();
		}
		else if (AI_enabled && Mathf.Abs(AI_target.position.x-this.transform.position.x) < target_swingDistance)
		{
			attack();
		}
	}

	private void attack()
	{
			//If the cooldown for attacking is done
			if (attack_counter == 0)
			{
				//Plays animation (overrides other animations)
				this.GetComponent<Animator>().Play("swing");

				//Reset delay on attacking so you can't spam it
				attack_counter = attack_cooldown;

				//Plays a soundfile at the player's position
				if (SFX_enabled) AudioSource.PlayClipAtPoint(SFX_sword, this.transform.position);

				//How far the raycast goes out (how far the sword reaches)
				float swingRange = .25f;

				//Variable that holds information on the Raycasts that will be performed
				RaycastHit2D swingInfo;

				if (this.transform.localScale.x == 1)
				{
					//Raycasts out from the left side of the player going left
					swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x - .1f, this.transform.position.y),new Vector2(-1f,0f),swingRange);
				}
				else// if (this.transform.localScale.x == -1)
				{
					//Raycasts out from the right side of the player going right
					swingInfo = Physics2D.Raycast(new Vector2(this.transform.position.x + .1f, this.transform.position.y),new Vector2(1f,0f),swingRange);
				}
				//If the collider found something
				if (swingInfo.collider != null)
				{
					//If the swinging hit an enemy
					if (swingInfo.collider.gameObject.GetComponent<Person_Health>() != null)
					{
						//Do damage to the enemy and run its animations
						swingInfo.collider.gameObject.GetComponent<Person_Health>().takeDamage();	
					}
				}				
			}
				
	}
}
