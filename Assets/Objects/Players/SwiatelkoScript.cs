using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SwiatelkoScript : MonoBehaviour
{

    public Animator lineAnim;
    public KeyCode actionKey;
    public float moveSpeed = 3f;
    public GameObject line;
    public bool lineActive;

    public bool canDropLine;
    public bool inAction;

    Rigidbody rb;

    public Rigidbody[] lineParts;

    void Start()
    {
        lineActive = false;
        rb = this.GetComponent<Rigidbody>();
        GetComponent<LoopedSoundAttachment>().Play();
        //GetComponent<AudioSource>().volume = 0.5f;
    }

    void Move()
    {
        rb.velocity = (Input.GetKey("d") == true ? moveSpeed : 0) * Vector3.right + (Input.GetKey("w") == true ? moveSpeed : 0) * Vector3.up +
            (Input.GetKey("a") == true ? moveSpeed : 0) * Vector3.left + (Input.GetKey("s") == true ? moveSpeed : 0) * Vector3.down;

    }


    IEnumerator StopSwingRoutine()
    {
        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (Rigidbody part in lineParts)
                {
                    part.velocity = new Vector3(0f, 0f, 0f);
                    part.AddForce(new Vector3(0f, -100f, 0f));
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StopSwing()
    {
        StartCoroutine(StopSwingRoutine());
    }

    public void DebugLineSpeed()
    {

        float maxi = 0.0f;
        foreach (Rigidbody part in lineParts)
        {
            if (part.velocity.magnitude > 2.6f)
                part.velocity /= 2;
            //part.AddForce(new Vector3(0f, -100f, 0f));
        }

    }

    void Update()
    {
        //Debug.Log(canDropLine);
        Move();
        //DebugLineSpeed();
        rb.useGravity = false;
        if (Input.GetKeyDown(actionKey) && !inAction && canDropLine)
        {
            StopSwing();
            lineActive = !lineActive;
            line.SetActive(lineActive);
            if (!lineActive)
            {
                GameObject.Find("Player 1").GetComponent<PlayerScript>().ToGround();
                rb.isKinematic = false;
                StopSwing();
            }
            else
            {
                StopSwing();
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LineZone")
        {
            canDropLine = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.gameObject.CompareTag("LineZone"))
        {
            canDropLine = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LineZone"))
        {
            canDropLine = false;
        }
    }

}
