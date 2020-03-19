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
    public void RestrictLeft ()
    {
        restrictLeft = true;
    }
    public void RestrictRigth ()
    {
        restrictRight = true;
    }
    public void UnrestrictLeft ()
    {
        restrictLeft = false;
    }
    public void UnrestrictRigth ()
    {
        restrictRight = false;
    }
    // ----------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        // changed while merging with camera movement
	if(Input.GetKey(right) && !restrictRight)
	{
	    transform.position += new Vector3(normalSpeed*Time.deltaTime,0f,0f);
	}
	if(Input.GetKey(left) && !restrictLeft)
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
