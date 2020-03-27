using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiatelkoScript : MonoBehaviour
{

    public Animator lineAnim;
    public KeyCode lineKey;
    public float moveSpeed = 3f;
    public GameObject line;
    bool lineActive;

    Rigidbody rb;

    void Start()
    {
        lineActive = false;
        rb = this.GetComponent<Rigidbody>();
    }

    void Move()
    {
        rb.velocity = (Input.GetKey("d") == true ? moveSpeed : 0) * Vector3.right + (Input.GetKey("w") == true ? moveSpeed : 0) * Vector3.up +
            (Input.GetKey("a") == true ? moveSpeed : 0) * Vector3.left + (Input.GetKey("s") == true ? moveSpeed : 0) * Vector3.down;

    }



    void Update()
    {
        Move();
        if(Input.GetKeyDown(lineKey))
        {
            lineActive = !lineActive;
            line.SetActive(lineActive);
            if(!lineActive)
            {
                GameObject.Find("Player 1").GetComponent<PlayerScript>().ToGround();
                rb.useGravity = false;
            }
        }
    }
}
