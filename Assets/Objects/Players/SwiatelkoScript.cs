using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SwiatelkoScript : MonoBehaviour
{

    public Animator lineAnim;
    public KeyCode actionKey;
    public float moveSpeed = 3f;
    public GameObject line;
    bool lineActive;

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


    void StopSwing()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (Rigidbody part in lineParts)
            {
                part.velocity = new Vector3(0f, 0f, 0f);
                part.AddForce(new Vector3(0f, -100f, 0f));
            }
        }
    }


    void Update()
    {
        Move();
        rb.useGravity = false;
        if (Input.GetKeyDown(actionKey) && !inAction)
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
