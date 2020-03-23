//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bulletExit;
    public GameObject pistol, bullet;

    float xMov;
    bool jump, shot;

    public float moveSpeed = 10f;
    public float jumpHeight = 1f;
    public float bulletSpeed = 100f;
    public float gravity = -9.81f;
    bool isGrounded;

    PlayerScript playerScript;

    public Material[] stateMaterials;

    Vector3 velocity;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
    }

    void CheckState()
    {
        if (playerScript.state == PlayerScript.State.running)
            GetComponent<MeshRenderer>().material = stateMaterials[0];
        else if(playerScript.state == PlayerScript.State.shooting)
            GetComponent<MeshRenderer>().material = stateMaterials[1];
    }

    void GetInput()
    {
        if(Input.GetButtonDown("StateButton"))
        {
            if (playerScript.state == PlayerScript.State.shooting)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                pistol.transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }

        if (Input.GetButtonDown("Fire1"))
            shot = true;
        else
            shot = false;
    }

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
        CheckState();
        if(playerScript.state == PlayerScript.State.shooting)
        {
            Shot();
        }

        //ApplyGravity();
    }
}
