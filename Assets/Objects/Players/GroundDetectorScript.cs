using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectorScript : MonoBehaviour
{
	public bool grounded;
	public PlayerScript ps;
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
		if(!col.gameObject.CompareTag("Player1") && !col.gameObject.CompareTag("InteractionSurrounding") && !col.gameObject.CompareTag("CameraWall") && !col.gameObject.CompareTag("CameraWallBuffer") && !col.gameObject.CompareTag("LineZone"))
		{
			//Debug.Log(col.gameObject.name);
			grounded = true;
			ps.SetGrounded(grounded);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if (!col.gameObject.CompareTag("Player1") && !col.gameObject.CompareTag("InteractionSurrounding") && !col.gameObject.CompareTag("CameraWall") && !col.gameObject.CompareTag("CameraWallBuffer") && !col.gameObject.CompareTag("LineZone"))
		{
			grounded = false;
			ps.SetGrounded(grounded);
		}
	}
	
}
