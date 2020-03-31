using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingScript : MonoBehaviour
{
    public float speed;
    private PlayerScript[] players = new PlayerScript[2];

    void Start()
    {
        players[0] = GameObject.FindWithTag("Player1").GetComponent<PlayerScript>();
	    players[1] = GameObject.FindWithTag("Player2").GetComponent<PlayerScript>();
    }

    void Update() {}

    void OnTriggerStay(Collider other)
    {
    	if (other.gameObject.CompareTag("Player1"))
    	{
    	    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
    	    other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

    	    if (Input.GetKey(players[0].up) && other.gameObject.CompareTag("Player1"))
    	    {
    	        other.gameObject.transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
    	    }

    	    if (Input.GetKey(players[0].down) && other.gameObject.CompareTag("Player1"))
    	    {
    	        other.gameObject.transform.position += new Vector3(0f, -speed * Time.deltaTime, 0f);
    	    }
    	}
    }

    void OnTriggerExit(Collider other)
    {
    	if (other.gameObject.CompareTag("Player1"))
    	    other.gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
