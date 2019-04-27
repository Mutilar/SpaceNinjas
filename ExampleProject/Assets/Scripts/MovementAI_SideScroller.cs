using UnityEngine;
using System.Collections;

public class MovementAI_SideScroller : MonoBehaviour {

	public float movement_xSpeed;
	
	public Transform AI_target;

	public bool target_found;
	public float target_detectionDistance;
	public float target_closestDistance;

	public bool target_forgettable;
	public float target_forgetDistance;

	public bool idle_patrolling;
	public Vector2[] idle_patrolPoints;

	private int idle_patrolPointNumber = 0;

	// Update is called once per frame
	void Update () 
	{
		if (target_found) 
		{
			float speed = Mathf.Clamp(AI_target.position.x - this.transform.position.x,-movement_xSpeed,movement_xSpeed);	

			//If moving...
			if (speed != 0) 
			{
				//Start walking animation
				this.GetComponent<Animator>().SetBool ("moving", true);

				//Move the player left at the rate of the speed variable
				if (Mathf.Abs(AI_target.position.x-this.transform.position.x) > target_closestDistance) this.transform.Translate(new Vector2(speed,0));

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

			if (target_forgettable && Mathf.Abs(AI_target.position.x-this.transform.position.x) > target_forgetDistance) target_found = false;
		}
		else
		{
			idleTask();
			if (Mathf.Abs(AI_target.position.x-this.transform.position.x) < target_detectionDistance) target_found = true;		
		}
	}

	void idleTask()
	{
		if (idle_patrolling) 
		{
			if (this.transform.position.x == idle_patrolPoints[idle_patrolPointNumber].x)
			{
				idle_patrolPointNumber++;
				if (idle_patrolPointNumber == idle_patrolPoints.Length) idle_patrolPointNumber = 0;
			}
			this.transform.Translate(new Vector2(Mathf.Clamp(idle_patrolPoints[idle_patrolPointNumber].x - this.transform.position.x,-movement_xSpeed,movement_xSpeed),0));
		}
	}
}

