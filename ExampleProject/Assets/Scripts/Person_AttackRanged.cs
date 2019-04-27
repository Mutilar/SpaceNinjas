using UnityEngine;
using System.Collections;

public class Person_AttackRanged : MonoBehaviour {

	int attack_cooldown = 50;
	//Counts down from "attackCooldown", making a delay between attacks
	int attack_counter = 0;

	public bool AI_enabled;

	public Transform AI_target;
	
	public float target_shootDistance;

	public GameObject projectile_prefab;

	int projectile_amount = 3;

	public bool UI_enabled;
	//GUI display for each life point
	public GameObject UI_projectile1;
	public GameObject UI_projectile2;
	public GameObject UI_projectile3;

	public bool SFX_enabled;
	
	public AudioClip SFX_shot;
	

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
		else if (AI_enabled && Mathf.Abs(AI_target.position.x-this.transform.position.x) < target_shootDistance)
		{
			attack();
		}
	}

	void attack()
	{
			//If the cooldown for attacking is done
			if (attack_counter == 0)
			{
				//If the player has enough stars to throw
				if (projectile_amount > 0)
				{
					//Plays animation (overrides other animations)
					this.GetComponent<Animator>().Play("throw");

					//Reset delay on attacking so you can't spam it
					attack_counter = attack_cooldown;

					//Makes a new object at the player's position
					GameObject projectile =  Instantiate(projectile_prefab, this.transform.position,this.transform.rotation) as GameObject;

					//Sets which way the object is going depending on which way the player is looking
					if (this.transform.localScale.x == 1) projectile.GetComponent<starPhysics>().goingLeft = true;

					//Plays a soundfile at the player's position
					if (SFX_enabled) AudioSource.PlayClipAtPoint(SFX_shot, this.transform.position);

					//Use up one star's worth of ammo
					projectile_amount -= 1;

					//Updates stars on the GUI
					if (projectile_amount == 2)
					{
						if (UI_enabled) UI_projectile3.SetActive(false);
					}
					if (projectile_amount == 1)
					{
						if (UI_enabled) UI_projectile2.SetActive(false);
					}
					if (projectile_amount == 0)
					{
						if (UI_enabled) UI_projectile1.SetActive(false);
					}
				}
			}
	}
}
