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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
	if(Input.GetKey(right))
	{
	    transform.position += new Vector3(normalSpeed*Time.deltaTime,0f,0f);
	}
	if(Input.GetKey(left))
	{
	    transform.position += new Vector3(-normalSpeed*Time.deltaTime,0f,0f);
	}
	if(Input.GetKeyDown(up) && !climbing && grounded)
	{
	    rb.AddForce(0f, jumpForce, 0f);
	}
        
    }
    void OnCollisionStay(Collision col)
    {
	//if(col.gameObject.CompareTag("Platform"))
	//{
	    grounded = true;
	//}
    }
    void OnCollisionExit(Collision col)
    {
	//if(col.gameObject.CompareTag("Platform"))
	//{
	    grounded = false;
	//}
    }
    
	    
}
