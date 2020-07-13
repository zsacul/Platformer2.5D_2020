using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectorScript : MonoBehaviour
{
	public bool grounded;
	public PlayerScript ps;

	private List<string> ignoreTags = new List<string>() {
		"Player1",
		"InteractionSurrounding",
		"CameraWall",
		"CameraWallBuffer",
		"LineZone",
		"line",
		"Stairs",
		"Player2",
		"Ladder" 
	};

    void Start()
    {
		ps = GameObject.FindWithTag("Player1").GetComponent<PlayerScript>();
		grounded = true;
    }

    void Update()
    {
        
    }
	
	void OnTriggerStay(Collider col)
	{
		if(!ignoreTags.Contains(col.gameObject.tag))
		{
			Debug.Log(col.gameObject.tag);
			//Debug.Log(col.gameObject.name);
			grounded = true;
			ps.SetGrounded(grounded);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (!ignoreTags.Contains(col.gameObject.tag))
		{
			Debug.Log(col.gameObject.tag);
			grounded = false;
			ps.SetGrounded(grounded);
		}
	}
	
}
