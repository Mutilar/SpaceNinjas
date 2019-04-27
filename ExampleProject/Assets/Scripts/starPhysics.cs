using UnityEngine;
using System.Collections;

public class starPhysics : MonoBehaviour 
{
	//Which way the star is/should be going
	public bool goingLeft;

	public bool rotating;

	//Provides starting modifiers to the object (moving it the correct direction in this case)
	void Start () 
	{
		//If the player was looking left, push the star left
		if (goingLeft == true) 
		{
			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-100f, 0));
		}
		//If the player was looking right, push the star right
		else 
		{
			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100f, 0));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (rotating == true)
		{
			//Rotate the star head-over-heels, this depends on which way the star is going
			if (goingLeft == true)
			{
				this.transform.Rotate (new Vector3 (0, 0, 5f));
			}
			else 
			{
				this.transform.Rotate (new Vector3 (0, 0, -5f));
			}
		}
	}
}
