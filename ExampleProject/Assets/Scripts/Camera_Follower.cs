using UnityEngine;
using System.Collections;

public class Camera_Follower : MonoBehaviour {

	//Transform of target to be followed
	public Transform follow_target;
	//Only follow on X value (preferred for giving depth to backgrounds)
	public bool follow_onlyX;

	//If Camera snaps to target every frame
	public bool style_snap;
	
	//If Camera trends towards target's position
	public bool style_smooth;
	//Speed at which Camera trends towards target
	public float style_smoothness;

	//Update is called once per frame
	void Update () 
	{
		//Camera exactly on target every frame
		if (style_snap) 
		{
			if (follow_onlyX)
			{
				//Set Camera's X position to Target's X position
 				this.transform.position = new Vector3(follow_target.position.x,this.transform.position.y,this.transform.position.z);	
			}
			else 
			{
				//Sets Camera's X and Y positions to Target's X and Y positions respectively
				this.transform.position = new Vector3(follow_target.position.x,follow_target.position.y,this.transform.position.z);	
			}
		}
		//Camera trends towards target position	
		if (style_smooth) 
		{
			if (follow_onlyX)
			{
				//Trends Camera's X position closer to Target's X position
 				this.transform.position = new Vector3(this.transform.position.x - (this.transform.position.x - follow_target.position.x) / style_smoothness, this.transform.position.y,this.transform.position.z);
			}
			else 
			{
				//Trends Camera's X and Y positions to Target's X and Y positions respectively
				this.transform.position = new Vector3(this.transform.position.x - (this.transform.position.x - follow_target.position.x) / style_smoothness, this.transform.position.y - ( this.transform.position.y - follow_target.position.y) / style_smoothness,this.transform.position.z);
			}
		}
	}
}
