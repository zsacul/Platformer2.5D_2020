using System;
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
            if (Math.Abs(other.gameObject.GetComponent<PlayerScript>().xMov) == 0)
            {
                if (Input.GetKey(players[0].up))
                {
                    other.gameObject.GetComponent<PlayerScript>().onLadder = true;
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    other.gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
                    other.gameObject.GetComponentInChildren<Animator>().Play("LadderClimb");
                    other.gameObject.transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
                    other.gameObject.GetComponentInChildren<Animator>().SetFloat("up", 1.0f);
                }
                else if (Input.GetKey(players[0].down))
                {
                    other.gameObject.GetComponent<PlayerScript>().onLadder = true;
                    other.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    other.gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
                    other.gameObject.GetComponentInChildren<Animator>().Play("LadderClimb");
                    other.gameObject.transform.position += new Vector3(0f, -speed * Time.deltaTime, 0f);
                    other.gameObject.GetComponentInChildren<Animator>().SetFloat("up", -1.0f);
                }
                else if (other.gameObject.GetComponent<PlayerScript>().grounded)
                {
                    other.gameObject.GetComponent<PlayerScript>().onLadder = false;
                    other.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    other.gameObject.GetComponentInChildren<Animator>().SetTrigger("endLadder");
                }
                else
                {
                    other.gameObject.GetComponentInChildren<Animator>().SetFloat("up", 0.0f);
                }
            }
    	}
		if(other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<Rigidbody>().useGravity = false;
    	    other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);

            if (other.gameObject.GetComponent<EnemyScript>())
                other.gameObject.GetComponent<EnemyScript>().ladder = true;
            else
                other.gameObject.GetComponent<WalkingEnemyScript>().ladder = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            other.gameObject.GetComponent<PlayerScript>().onLadder = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
            other.gameObject.GetComponentInChildren<Animator>().SetTrigger("endLadder");
        }
		if (other.gameObject.CompareTag("Enemy"))
		{
    	    other.gameObject.GetComponent<Rigidbody>().useGravity = true;

            if (other.gameObject.GetComponent<EnemyScript>())
                other.gameObject.GetComponent<EnemyScript>().ladder = false;
            else
                other.gameObject.GetComponent<WalkingEnemyScript>().ladder = false;

        }
    }
}
