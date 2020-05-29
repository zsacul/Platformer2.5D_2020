using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionDetector : MonoBehaviour
{

    public CameraScript cs;
	public bool collision = false;
	public bool falling = false;

	void Start()
	{
		cs = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
	}

    void OnTriggerStay(Collider col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			//Debug.Log("wall collision" + col.gameObject.name);
			//cs.SetZPosition(-0.2f);
			cs.wallCollision = true;
			collision = true;
		}
		if(col.gameObject.CompareTag("Player1"))
        {
			falling = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			//Debug.Log("collision exit" + col.gameObject.name);
			//cs.SetZPosition(0.2f);
			cs.wallCollision = false;
			collision = false;
		}
		if (col.gameObject.CompareTag("Player1"))
		{
			falling = false;
		}
	}
}
