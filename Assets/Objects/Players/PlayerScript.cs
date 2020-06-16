using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;

public class PlayerScript : MonoBehaviour
{
    bool escapingLine;
    public GameObject Player2;

    public float normalSpeed;
    public float jumpForce;
    public float lineClimbSpeed;
    //public float friction = 0.98f;
    public float powerDist = 2f;

    public float hidingSpotDist;

    float xMov;
    bool jump;
    public bool inStairs, onLine;

    public KeyCode lineHoldKey;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public KeyCode jumpButton;
    public KeyCode actionKey;

    public UnityEvent jumpEvent;

    private Rigidbody rb;
    private lineHoldHelper lineHoldHelp;
    private Vector3 frozenHidingPosition;

    Animator anim;

    [HideInInspector]
    public enum State { running, shooting, climbing, hiding };
    public State state;

    public bool getting_power = false;
    public bool climbing = false;
    public bool grounded = true;

    public bool isSeen = false;

    private GameObject currentLinePart;

    public void hidePlayer()
    {
        if (state != State.hiding)
        {
            state = State.hiding;
            anim.SetBool("hiding", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
            rb.velocity = Vector3.zero;
            anim.SetFloat("velocity", 0);
            frozenHidingPosition = transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + hidingSpotDist);
        }
    }

    public void unhidePlayer()
    {
        if (state == State.hiding)
        {
            state = State.running;
            anim.SetBool("hiding", false);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - hidingSpotDist);
        }
    }

    void Awake()
    {
        lineHoldHelp = GetComponentInChildren<lineHoldHelper>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        Player2 = GameObject.FindWithTag("Player2");
        foreach(Collider collider in GetComponents<Collider>())
            Physics.IgnoreCollision(collider, GameObject.FindWithTag("Player2").GetComponent<Collider>());
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
        if (state != State.hiding)
        {
            xMov = Input.GetAxis("Horizontal");
            if (Input.GetKeyDown(jumpButton))
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
        else
        {
            rb.position = frozenHidingPosition;
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
            if (jumpEvent != null)
            {
                jumpEvent.Invoke();
            }
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

    /*void OnCollisionStay(Collision other)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = true;
        anim.SetTrigger("ground");
    }

    void OnCollisionExit(Collision other)
    {
	//if (col.gameObject.CompareTag("Platform"))
	    grounded = false;
        
    }*/
    public void SetGrounded(bool gr) // function used by GroundDetector
    {
        grounded = gr;
        escapingLine = false;
        if (grounded)
            anim.SetTrigger("ground");
    }

    public void ToGround()
    {
        onLine = false;
        rb.useGravity = true;
        state = State.running;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        //anim.SetTrigger("ground");
    }

    void ToLine()
    {
        onLine = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        state = State.climbing;
        anim.SetTrigger("climb");
    }

    public void Cought()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*Vector3 lastCheckpoint = GetComponent<CheckpointScript>().lastPos;
        transform.position = lastCheckpoint;
        Player2.transform.position = new Vector3(lastCheckpoint.x - 0.5f, lastCheckpoint.y + 1f, lastCheckpoint.z);
        */
    }

    void OnTriggerStay(Collider other)
    {
        float rotationSmooth = 0.5f;

        if (other.tag == "line")
        {
            if (!escapingLine)
            {
                //transform.position = new Vector3( lineHoldHelp.currentLinePart.transform.position.x, transform.position.y, transform.position.z);
                transform.position = new Vector3(Vector3.Lerp(transform.position, other.transform.position, 0.6f).x, transform.position.y, transform.position.z);
                //transform.rotation = Quaternion.Lerp(transform.rotation, other.gameObject.transform.rotation, rotationSmooth);

                ToLine();

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.position += (Player2.transform.position - transform.position).normalized / 3 * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.position -= (Player2.transform.position - transform.position).normalized / 3 * Time.deltaTime;
                }

                if (xMov < 0)//swinging rope
                    other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 100f);
                else if (xMov > 0)
                    other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 100f);
            }

            if (Input.GetKey(lineHoldKey))
            {
                ToGround();
                escapingLine = true;
            }
            Player2.GetComponent<SwiatelkoScript>().DebugLineSpeed();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (Input.GetKey(KeyCode.DownArrow) && other.gameObject.CompareTag("line"))
        {
            ToGround();
            //escapingLine = true;
        }
    }

}
