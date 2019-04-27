using UnityEngine;
using System.Collections;

public class Movement_TopDown : MonoBehaviour {

	public float movement_xSpeed;
	public float movement_ySpeed;

	public bool movement_WASD;
	public bool movement_followMouse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (movement_WASD) this.transform.Translate(new Vector2(Input.GetAxis("Horizontal") * movement_xSpeed,Input.GetAxis("Vertical") * movement_ySpeed));
		if (movement_followMouse) this.transform.Translate(new Vector2(Mathf.Clamp(Camera.main.ScreenToWorldPoint (Input.mousePosition).x - this.transform.position.x,-movement_xSpeed,movement_xSpeed), Mathf.Clamp(Camera.main.ScreenToWorldPoint (Input.mousePosition).y - this.transform.position.y,-movement_ySpeed,movement_ySpeed)));
	}
}
