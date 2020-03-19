//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bulletExit;
    public GameObject pistol, bullet, testObject;

    float xMov;
    bool jump, shot;

    public float moveSpeed = 10f;
    public float jumpHeight = 1f;
    public float bulletSpeed = 100f;
    public float gravity = -9.81f;
    bool isGrounded;

    public Material[] stateMaterials;

    Vector3 velocity;

    enum State { running, shooting };
    State state;
    public float testTime;

    void Start()
    {
        testTime = Time.time;
    }

    // void ApplyGravity()
    // {
    //     velocity.y += gravity * Time.deltaTime;
    //     controller.Move(velocity * Time.deltaTime);
    // }

    // void CheckGround()
    // {
    //     isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
    //     if (isGrounded && velocity.y < 0f)
    //     {
    //         velocity.y = -2f;
    //     }
    // }

    void CheckState()
    {
        if (state == State.running)
            GetComponent<MeshRenderer>().material = stateMaterials[0];
        else if(state == State.shooting)
            GetComponent<MeshRenderer>().material = stateMaterials[1];
    }

    void GetInput()
    {
        // xMov = Input.GetAxis("Horizontal");
        // if (Input.GetButtonDown("Jump"))
        //     jump = true;
        // else jump = false;

        if (Input.GetButtonDown("StateButton"))
        {
            if (state == State.running)
            {
                state = State.shooting;
            }
            else if (state == State.shooting)
            {
                state = State.running;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                pistol.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            }
        }

        if (Input.GetButtonDown("Fire1"))
            shot = true;
        else
            shot = false;
    }

    // void Move()
    // {
    //     if (xMov < 0)
    //         transform.eulerAngles = new Vector3(0, -180, 0);
    //     else if (xMov > 0)
    //         transform.eulerAngles = new Vector3(0, 0, 0);

    //     Vector3 move = transform.right * Math.Abs(xMov);
    //     controller.Move(move * moveSpeed * Time.deltaTime);

    //     if (isGrounded && jump)
    //     {
    //         velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    //     }
    // }

    void Shot()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 pos = Camera.main.ScreenToWorldPoint(mouse);
        pistol.transform.LookAt(pos);

        if (shot)
        {
            GameObject newBullet;
            newBullet = Instantiate(bullet, bulletExit.position, pistol.transform.rotation);
            Vector3 shotDir = (pos - bulletExit.position).normalized; 
            newBullet.GetComponent<Rigidbody>().AddForce(shotDir * bulletSpeed);
        }

        if ((pos - bulletExit.position).x < 0)
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        else
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

    }

    void Update()
    {
        GetInput();
        //CheckGround();
        CheckState();

        if (state == State.running)
        {
            //Move();
        }
        else if(state == State.shooting)
        {
            Shot();
        }

        //ApplyGravity();
    }
}
