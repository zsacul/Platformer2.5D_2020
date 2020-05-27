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


    void StopSwing()
    {
        foreach(Rigidbody part in lineParts)
        {
            part.velocity = new Vector3(0f, 0f, 0f);
            part.AddForce(new Vector3(0f, -100f, 0f));
        }
    }


    void Update()
    {
        Move();
        rb.useGravity = false;
        if (Input.GetKeyDown(lineKey))
        {
            lineActive = !lineActive;
            line.SetActive(lineActive);
            if(!lineActive)
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
}
