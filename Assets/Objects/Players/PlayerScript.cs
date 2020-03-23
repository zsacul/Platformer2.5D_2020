using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    
    public float normalSpeed;
    public float jumpForce;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    private Rigidbody rb;

    public bool climbing = false;
    public bool grounded = true;

    // added while merging with camera movement
    private bool restrictLeft = false;
    private bool restrictRight = false;
    private bool restrictUp = false;
    private bool restrictDown = false;

    public void RestrictLeft()
    {
        restrictLeft = true;
    }

    public void RestrictRigth()
    {
        restrictRight = true;
    }

    public void RestrictUp()
    {
        restrictUp = true;
    }

    public void RestrictDown()
    {
        restrictDown = true;
    }

    public void UnrestrictLeft()
    {
        restrictLeft = false;
    }

    public void UnrestrictRigth()
    {
        restrictRight = false;
    }

    public void UnrestrictUp()
    {
        restrictUp = false;
    }

    public void UnrestrictDown()
    {
        restrictDown = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float vx = rb.velocity.x;
        float vy = rb.velocity.y;

        // changed while merging with camera movement
    	if (Input.GetKey(right) && !restrictRight)
    	    //transform.position += new Vector3(normalSpeed * Time.deltaTime, 0f, 0f);
            vx = normalSpeed; // for smoother movement

    	if (Input.GetKey(left) && !restrictLeft)
    	    //transform.position += new Vector3(-normalSpeed * Time.deltaTime, 0f, 0f);
            vx = -normalSpeed; // for smoother movement

    	if (Input.GetKeyDown(up) && !climbing && grounded)
    	    rb.AddForce(0f, jumpForce, 0f);


        if (restrictLeft && rb.velocity.x < 0)
            vx = 0f;

        if (restrictRight && rb.velocity.x > 0) 
            vx = 0f;

        if (restrictUp && rb.velocity.y > 0)
            vy = 0f;

        if (restrictDown && rb.velocity.y < 0) 
            vy = 0f;

        rb.velocity = new Vector3(vx, vy, 0f);// rb.velocity.z); ?
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
    
	    
}
