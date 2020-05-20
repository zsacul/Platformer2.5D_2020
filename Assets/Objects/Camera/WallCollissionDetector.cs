using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollissionDetector : MonoBehaviour
{

    public CameraScript cs;
	public bool collision = false;

	void Start()
	{
		cs = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
	}

    void OnCollisionStay(Collision col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			Debug.Log("enter" + col.gameObject.name);
			//cs.SetZPosition(-0.2f);
			cs.wallCollision = true;
			collision = true;
		}
    }

    void OnCollisionExit(Collision col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			Debug.Log("exit" + col.gameObject.name);
			//cs.SetZPosition(0.2f);
			cs.wallCollision = false;
			collision = false;
		}
    }
}
