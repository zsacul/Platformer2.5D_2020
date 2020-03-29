using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
    public float normalSpeed;
    public float jumpForce;
    public float lineClimbSpeed;
    public float friction = 0.98f;

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


    public bool climbing = false;
    public bool grounded = true;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
		Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindWithTag("Player2").GetComponent<Collider>());
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
        if (state == State.running)
            Move();
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
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = 0;
        state = State.climbing;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "line" && Input.GetKey(lineHoldKey))
        {
            ToLine();
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "line" && Input.GetKey(lineHoldKey) && !jump)
        {
            ToLine();

            transform.rotation = Quaternion.Lerp(transform.rotation, other.gameObject.transform.rotation, 0.5f);

            Vector3 vel = Vector3.zero;
            vel.x += (other.gameObject.transform.position.x - transform.position.x) * 7f;//for player to hold onto line x

            if (Input.GetKey(KeyCode.UpArrow))
            {
                vel += transform.up.normalized * lineClimbSpeed; //climbing up
            }
            else if ((other.gameObject.transform.position - transform.position).magnitude > 0.05f && Math.Abs(other.transform.position.y - transform.position.y) < 0.6f)
            {
                vel.y += (other.transform.position.y - transform.position.y) * lineClimbSpeed;//for player to hold onto line y
            }
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
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "line")
        {
            ToGround();
        }
    }




}
