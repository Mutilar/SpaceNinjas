using UnityEngine;
using System.Collections;

public class islandMovement : MonoBehaviour {

	bool movingLeft;
	// Use this for initialization
	void Start () 
	{
		movingLeft = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (movingLeft) 
		{
			if (transform.position.x < 7) movingLeft = false;
			transform.Translate(new Vector2(-.01f, 0));
		}
		else
		{
			if (transform.position.x > 8.5) movingLeft = true;
			transform.Translate(new Vector2(.01f, 0));
		}

	}
}
