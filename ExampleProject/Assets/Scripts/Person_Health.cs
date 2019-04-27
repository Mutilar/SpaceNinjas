using UnityEngine;
using System.Collections;

public class Person_Health : MonoBehaviour {

	//How many hits you can take
	int life_amount = 3;

	public bool UI_enabled;
	//GUI display for each life point
	public GameObject UI_heart1;
	public GameObject UI_heart2;
	public GameObject UI_heart3;

	public bool SFX_enabled;
	//Sound effect played when hurt
	public AudioClip SFX_damage;

	//On death, reset level
	public bool death_reset;
	//On death, delete person
	public bool death_deletion;
	
	// Update is called once per frame
	void Update () 
	{
		//Resets player's sprite color
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 255, 255);

		//If too low, die
		//NOTE: if game seems to freeze, the issue might be that the player is spawning below the "-1", and constantly resetting
		if (this.transform.position.y < -1f)
			die ();
	}

	//Trigger Enter called every time an object with a trigger collider contacts your collider 
	//Note that enemy AIs wont hurt you this way due to the fact that they do not have trigger colliders
	void OnTriggerEnter2D(Collider2D other)
	{    
		//If the thing you touched is "tagged" as an enemy
		if ((this.gameObject.tag == "friend" && other.gameObject.tag == "enemy") || (this.gameObject.tag == "enemy" && other.gameObject.tag == "friend")) 
		{
			//Update hearts on the GUI
			takeDamage ();
		}
	}

	//Take Damage called whenever the AI takes a hit of any sort
	public void takeDamage()
	{ 
		//Turns the player red (flash of red as it is reset on the next frame)
		this.GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0);

		//Plays a soundfile at the player's position
		if (SFX_enabled) AudioSource.PlayClipAtPoint(SFX_damage, this.transform.position);

		//Takes damage, one point of life
		life_amount -= 1;

		//Updates the hearts on the GUI
		if (life_amount == 2) 
		{
			if (UI_enabled) UI_heart3.SetActive(false);
		}
		if (life_amount == 1) 
		{
			if (UI_enabled) UI_heart2.SetActive(false);
		}
		if (life_amount == 0) 
		{
			//If the player has no more life, the level is reseted
			die();
		}

	}

	//Die called when the player has no more life
	public void die()
	{
		if (death_reset)
		{
			//Simply resets the scene to play again
			Application.LoadLevel ("world");
		}
		if (death_deletion)
		{
			//Deletes person
			Destroy (this.gameObject);
		}

	}
}
