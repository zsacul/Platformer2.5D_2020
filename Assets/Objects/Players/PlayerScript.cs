﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public GameObject Player2;

    public float normalSpeed;
    public float jumpForce;
    public float lineClimbSpeed;
    //public float friction = 0.98f;
    public float powerDist = 2f;

    float xMov;
    bool jump;

    public KeyCode lineHoldKey;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    private Rigidbody rb;

    [HideInInspector]
    public enum State { running, shooting, climbing };
    public State state;

    public bool getting_power = false;
    public bool climbing = false;
    public bool grounded = true;

    private GameObject currentLinePart;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Player2 = GameObject.Find("Player 2");
		Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player2").GetComponent<Collider>());
    }

    void CheckForce()
    {
        if (Vector3.Distance(Player2.transform.position, this.transform.position) < powerDist)
        {
            getting_power = true;
        }
        else
        {
            getting_power = false;
        }
    }

    void GetInput()
    {
        xMov = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            jump = true;
        else jump = false;

        if (Input.GetButtonDown("StateButton"))
        {
            if (state == State.running)
            {
                state = State.shooting;
            }
            else if (state == State.shooting)
            {
                state = State.running;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    void Move()
    {
        if (xMov < 0)
            transform.eulerAngles = new Vector3(0, -180, 0);
        else if (xMov > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        Vector3 move = transform.right * Math.Abs(xMov) * normalSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;

        if (jump && grounded)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
        }
    }


    void Update()
    {
        GetInput();
        CheckForce();
        if (state == State.running)
            Move();

        //Debug.Log(getting_power);
    }

    void OnCollisionStay(Collision col)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = true;
    }

    void OnCollisionExit(Collision col)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = false;
    }

    public void ToGround()
    {
        rb.useGravity = true;
        state = State.running;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }

    void ToLine()
    {
        //rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        state = State.climbing;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "line" && Input.GetKey(lineHoldKey))
        {
            ToLine();
            //rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (currentLinePart == null || currentLinePart == other.gameObject || (currentLinePart.transform.position.y <= other.gameObject.transform.position.y && Input.GetKey(KeyCode.UpArrow)))
        {
            currentLinePart = other.gameObject;
            if (other.gameObject.tag == "line" && Input.GetKey(lineHoldKey) && !jump)
            {
                ToLine();
                
                float deadX = 0.15f;
                transform.rotation = Quaternion.Lerp(transform.rotation, other.gameObject.transform.rotation, 0.5f);

                Vector3 vel = rb.velocity;
                

                if (!Input.GetKey(KeyCode.UpArrow))
                {
                    
                    vel = Vector3.zero;
                    Vector3 destination = other.gameObject.transform.position;// + offset;
                    Vector3 smoothedPosition = Vector3.Lerp(transform.position, destination, 0.5f);
                    transform.position = smoothedPosition;
                }
                else 
                {
                    vel.y = transform.up.y * lineClimbSpeed; //climbing up

                    if (Math.Abs(other.gameObject.transform.position.x - transform.position.x) > deadX)
                    {
                        vel.x += (other.gameObject.transform.position - transform.position).x / (Time.fixedDeltaTime * 10);//for player to hold onto line on x
                    }
                    else
                        vel.x *= 0.6f;

                    if (Math.Abs(vel.x) > 2.5f)
                        vel.x = vel.x > 0 ? 2.5f : -2.5f;
                }

                vel.z = 0;

                rb.velocity = vel;

                if (xMov < 0)//swinging rope
                    other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 100f);
                else if (xMov > 0)
                    other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 100f);

            }
            else if (other.gameObject.tag == "line" && !Input.GetKey(lineHoldKey))
            {
                rb.useGravity = true;
                state = State.running;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
                currentLinePart = null;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "line")
        {
            currentLinePart = null;
            ToGround();
        }
    }




}
