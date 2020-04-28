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
	
	void OnTriggerEnter(Collider col)
	{
		if(!col.gameObject.CompareTag("Player1") && !col.gameObject.CompareTag("Door"))
		{
			Debug.Log(col.gameObject.name);
			grounded = true;
			ps.SetGrounded(grounded);
		}
	}
	void OnTriggerExit(Collider col)
	{
		if(!col.gameObject.CompareTag("Player1"))
		{
			grounded = false;
			ps.SetGrounded(grounded);
		}
	}
	
}
