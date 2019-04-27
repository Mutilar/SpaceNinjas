using UnityEngine;
using System.Collections;

public class Movement_SideScroller : MonoBehaviour {
	
	public float movement_xSpeed;
	public float movement_jumpPower;

	// Update is called once per frame
	void Update ()
	{
			
		//Utilizes built-in key detection (supports "A" and "D", arrow keys, and joystick controls)
		float speed = Input.GetAxis("Horizontal");		

		//If moving...
		if (speed != 0) 
		{
			//Start walking animation
			this.GetComponent<Animator>().SetBool ("moving", true);

			//Move the player left at the rate of the speed variable
			transform.Translate(new Vector2(speed * movement_xSpeed,0));

			//Flip sprite according to direction it is moving
			if (speed > 0) 
			{
				transform.localScale = new Vector2(-1f,1f);
			}
			else 
			{
				transform.localScale = new Vector2(1f,1f);
			}
		}
		//If not moving, stop walking animation
		else 
		{
			this.GetComponent<Animator>().SetBool ("moving", false);
		}

		//Jump if keydown and something is below player to "jump" off of.
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
		{
			if (checkGrounded() == true)
			{
				//AddForce forces the player upwards
				this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,movement_jumpPower));
			}
		}

	}

	private bool checkGrounded()
	{
		//Distance down from player to check
		float detectionRange = .1f;

		//Raycasts out from the bottom of the player going down
		//IMPORTANT: "this.transform.position.y -.1" is to start the raycast below player's own collider. If the offset is too small, the raycast will only hit the player.
		RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y - .1f),new Vector2(0,-1f),detectionRange);			
	
		//If there is ground below the player (assume no at first)
		bool grounded = false;

		//If the raycast hits something below the player, it is "grounded" and can jump again
		if (hitInfo.collider != null)
		{
			grounded = true;	
		}

		return grounded;
	}
}
