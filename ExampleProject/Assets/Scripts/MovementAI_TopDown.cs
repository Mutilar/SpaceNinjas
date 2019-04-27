using UnityEngine;
using System.Collections;

public class MovementAI_TopDown : MonoBehaviour {

	public float movement_xSpeed;
	public float movement_ySpeed;

	public Transform AI_target;

	public bool target_found;
	public float target_detectionDistance;

	public bool target_forgettable;
	public float target_forgetDistance;

	public bool idle_patrolling;
	public Vector2[] idle_patrolPoints;

	private int idle_patrolPointNumber = 0;

	// Update is called once per frame
	void Update () {
	
		if (target_found) 
		{
			this.transform.Translate(new Vector2(Mathf.Clamp(AI_target.position.x - this.transform.position.x,-movement_xSpeed,movement_xSpeed), Mathf.Clamp(AI_target.position.y - this.transform.position.y,-movement_ySpeed,movement_ySpeed)));
			if (target_forgettable && Mathf.Sqrt(Mathf.Pow(AI_target.position.x-this.transform.position.x,2)+Mathf.Pow(AI_target.position.y-this.transform.position.y,2)) > target_forgetDistance) target_found = false;
		}
		else
		{
			idleTask();
			if (Mathf.Sqrt(Mathf.Pow(AI_target.position.x-this.transform.position.x,2)+Mathf.Pow(AI_target.position.y-this.transform.position.y,2)) < target_detectionDistance) target_found = true;		
		}
	}

	void idleTask()
	{
		if (idle_patrolling) 
			{
			if (this.transform.position.x == idle_patrolPoints[idle_patrolPointNumber].x && this.transform.position.y == idle_patrolPoints[idle_patrolPointNumber].y)
			{
				idle_patrolPointNumber++;
				if (idle_patrolPointNumber == idle_patrolPoints.Length) idle_patrolPointNumber = 0;
			}
			this.transform.Translate(new Vector2(Mathf.Clamp(idle_patrolPoints[idle_patrolPointNumber].x - this.transform.position.x,-movement_xSpeed,movement_xSpeed), Mathf.Clamp(idle_patrolPoints[idle_patrolPointNumber].y - this.transform.position.y,-movement_ySpeed,movement_ySpeed)));
		}
	}
}
