using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class SwiatelkoScript : MonoBehaviour
{
    public float nextLineDrop = 0;
    public float nextLineHide = 0;
    public float lineActiveTime = 3.0f;
    public float lineCooldown = 1.0f;


    public UnityEvent lineOnOff;
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
            if (part.velocity.magnitude > 3.0f)
                part.velocity *= 0.75f;
            //part.AddForce(new Vector3(0f, -100f, 0f));
        }

    }
    void Update()
    {
        //Debug.Log(canDropLine);
        Move();
        //DebugLineSpeed();
        rb.useGravity = false;

        if (lineActive && Time.time > nextLineDrop && GameObject.Find("Player 1").GetComponent<PlayerScript>().onLine)
        {
            lineActive = false;
            GameObject.Find("Player 1").GetComponent<PlayerScript>().ToGround();
            line.SetActive(lineActive);
            //rb.isKinematic = false;
            StopSwing();
            nextLineDrop = Time.time + lineCooldown;
            if (lineOnOff != null)
                lineOnOff.Invoke();
        }

        if (Input.GetKeyDown(actionKey) && !inAction && canDropLine)
        {
            if(Time.time > nextLineDrop)
            {
                StopSwing();
                lineActive = true;
                line.SetActive(lineActive);
                StopSwing();
                //rb.isKinematic = true;
                nextLineDrop = Time.time + lineActiveTime;
            }

            if (lineOnOff != null)
                lineOnOff.Invoke();
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
