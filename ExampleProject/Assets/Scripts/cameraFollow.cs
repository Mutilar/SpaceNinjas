using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public Transform player;
	public bool onlyX;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (onlyX) this.transform.position = new Vector3 (player.transform.position.x, this.transform.position.y,10f);
		else this.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y,-10f);
	}
}
