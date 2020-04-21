using System;
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
    private lineHoldHelper lineHoldHelp;

	Animator anim;

    [HideInInspector]
    public enum State { running, shooting, climbing };
    public State state;

    public bool getting_power = false;
    public bool climbing = false;
    public bool grounded = true;

    public bool isHiding = false;
    public bool isSeen = false;

    private GameObject currentLinePart;

    public void hidePlayer ()
    {
        isHiding = true;
        anim.SetBool("hiding", true);
    }

    public void unhidePlayer ()
    {
        isHiding = false;
        anim.SetBool("hiding", false);
    }

    void Awake()
    {
        lineHoldHelp = GetComponentInChildren<lineHoldHelper>();
		anim = GetComponentInChildren<Animator> ();
        rb = GetComponent<Rigidbody>();
        Player2 = GameObject.FindWithTag("Player2");
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
            anim.SetTrigger("jump");
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
        }
        anim.SetFloat("velocity", Math.Abs(rb.velocity.x));
    }


    void Update()
    {
        GetInput();
        CheckForce();
        if (state == State.running)
            Move();

        //Debug.Log(getting_power);
    }

    void OnCollisionStay(Collision other)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = true;
        anim.SetTrigger("ground");
    }

    void OnCollisionExit(Collision other)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = false;
        
    }

    public void ToGround()
    {
        rb.useGravity = true;
        state = State.running;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        //anim.SetTrigger("ground");
    }

    void ToLine()
    {
        //rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        state = State.climbing;
        anim.SetTrigger("climb");
    }

    void OnTriggerEnter(Collider other)
    {
        if (lineHoldHelp.canCatch && Input.GetKey(lineHoldKey))
        {
            ToLine();
            rb.velocity = Vector3.zero;
        }

        if(other.tag == "ladder")
        {
            anim.SetTrigger("climb");
        }
    }

    void OnTriggerStay(Collider other)
    {
       
        if (lineHoldHelp.canCatch && Input.GetKey(lineHoldKey) && !jump)
        {
            ToLine();

            float deadX = 0.15f;
            transform.rotation = Quaternion.Lerp(transform.rotation, other.gameObject.transform.rotation, 0.5f);

            Vector3 vel = rb.velocity;


            if (!Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetBool("arrow_up", false);
                vel = Vector3.zero;
                Vector3 destination = lineHoldHelp.currentLinePart.transform.position;// + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, destination, 1);
                transform.position = smoothedPosition;
            }
            else
            {
                anim.SetBool("arrow_up", true);
                vel.y = transform.up.y * lineClimbSpeed; //climbing up

                if (Math.Abs(other.gameObject.transform.position.x - transform.position.x) > deadX)
                {
                    vel.x += (other.gameObject.transform.position - transform.position).x / (Time.fixedDeltaTime);//for player to hold onto line on x
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

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "line")
        {
            currentLinePart = null;
            ToGround();
        }
    }




}
