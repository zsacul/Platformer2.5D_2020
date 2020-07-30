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

    public bool moving;

    public UnityEvent lineOn;
    public UnityEvent lineOff;
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

        if (Input.GetKey("d") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("w"))
            moving = true;
        else
            moving = false;


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
                    //part.AddForce(new Vector3(0f, -100f, 0f));
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

        if(lineActive && canDropLine == false)
        {
            lineActive = false;
            if(GameObject.Find("Player 1").GetComponent<PlayerScript>().onLine)
                GameObject.Find("Player 1").GetComponent<PlayerScript>().ToGround();
            //Debug.Log("B");
            line.SetActive(lineActive);
            //rb.isKinematic = false;
            //StopSwing();
            nextLineDrop = Time.time; //+ lineCooldown;
            if (lineOff != null)
                lineOff.Invoke();
        }

        if(lineActive && GameObject.Find("Player 1").GetComponent<PlayerScript>().onLine)
        {
            foreach(SpriteRenderer linePart in GetComponentsInChildren<SpriteRenderer>())
            {
                linePart.color = Color.Lerp(linePart.color, Color.gray, 0.03f);
            }
        }
        else
        {
            foreach (SpriteRenderer linePart in GetComponentsInChildren<SpriteRenderer>())
            {
                linePart.color = new Color(255, 255, 255, 255);
            }
        }


        if ((lineActive && Time.time > nextLineDrop && GameObject.Find("Player 1").GetComponent<PlayerScript>().onLine) || (canDropLine == false && GameObject.Find("Player 1").GetComponent<PlayerScript>().onLine))
        {
            lineActive = false;
            GameObject.Find("Player 1").GetComponent<PlayerScript>().ToGround();
            //Debug.Log("B");
            line.SetActive(lineActive);
            //rb.isKinematic = false;
            //StopSwing();
            nextLineDrop = Time.time; //+ lineCooldown;
            if (lineOff != null)
                lineOff.Invoke();
        }

        if (Input.GetKeyDown(actionKey) && !inAction && canDropLine)
        {
            if(Time.time > nextLineDrop)
            {
                StopSwing();
                lineActive = true;
                line.SetActive(lineActive);
                //StopSwing();
                //rb.isKinematic = true;
                nextLineDrop = Time.time + lineActiveTime;
            }

            if (lineOn != null)
                lineOn.Invoke();
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach(Collider col in GetComponents<Collider>())
                Physics.IgnoreCollision(collision.collider, col);
        }
    }

}
