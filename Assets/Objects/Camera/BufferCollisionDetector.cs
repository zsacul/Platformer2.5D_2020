using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferCollisionDetector : MonoBehaviour
{
    public CameraScript cs;

	void Start()
	{
		cs = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
	}

    void OnTriggerStay(Collider col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			//Debug.Log("enter" + col.gameObject.name);
			//cs.SetZPosition(-0.2f);
			cs.bufferCollision = true;
		}
    }

    void OnTriggerExit(Collider col)
    {
		if(col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
		{
			//Debug.Log("exit" + col.gameObject.name);
			//cs.SetZPosition(0.2f);
			cs.bufferCollision = false;
		}
    }
}
