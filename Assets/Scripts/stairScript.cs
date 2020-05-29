using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairScript : MonoBehaviour
{

    public GameObject other;
    public Animator fadeAnim;
	public CameraScript cs;
	
	void Start()
	{
		cs = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
	}

    IEnumerator fadeRoutine(Collider col)
    {
        fadeAnim.Play("screenFadeIn");
        yield return new WaitForSeconds(0.2f);
        Vector3 playerPos = col.gameObject.GetComponent<Transform>().position;
        col.gameObject.GetComponent<Transform>().position = new Vector3(other.transform.position.x, other.transform.position.y, playerPos.z);
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player1")
        {
            PlayerScript playerScript = col.gameObject.GetComponent<PlayerScript>();
            playerScript.inStairs = true;

            if (Input.GetKey(playerScript.actionKey))
            {
                if (cs.Stairs(other))
                    StartCoroutine(fadeRoutine(col));
                else
                    CannotUseStairs();
            }
        }
        
        if (col.gameObject.tag == "Player2")
        {
            SwiatelkoScript swiatelkoScript = col.gameObject.GetComponent<SwiatelkoScript>();
            swiatelkoScript.inAction = true;

            if (Input.GetKey(swiatelkoScript.actionKey))
            {
                if (cs.Stairs(other))
                    StartCoroutine(fadeRoutine(col));
                else
                    CannotUseStairs();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player1")
        {
            PlayerScript playerScript = col.gameObject.GetComponent<PlayerScript>();
            playerScript.inStairs = false;
        }

        if (col.gameObject.tag == "Player2")
        {
            SwiatelkoScript swiatelkoScript = col.gameObject.GetComponent<SwiatelkoScript>();
            swiatelkoScript.inAction = false;
        }
    }

    void CannotUseStairs()
    {
        //failure sound
        UnityEngine.Debug.Log("za daleko te schody");
    }
}
