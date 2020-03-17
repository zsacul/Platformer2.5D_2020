using UnityEngine;

public class PlayerEdges : MonoBehaviour
{
    public KeyCode up, left, right;
    public bool moveLeft, moveRight, moveUp, moveDown;
    float horizontalSpeed = 4f;
    bool grounded;
    float jumpSpeed = 6f;

    void Start() 
    {
        grounded = true;
        moveUp = true;
        moveLeft = true;
        moveRight = true;
        moveDown = true;
    }

    void Update()
    {
        float vy = GetComponent<Rigidbody>().velocity.y;
        float vx = 0f;

        if (Input.GetKeyDown(up) && grounded)
        {
            vy = jumpSpeed;
            grounded = false;
        }        

        if (Input.GetKey(left))
            vx = -horizontalSpeed;
        
        if (Input.GetKey(right))
            vx = horizontalSpeed;



        if (!moveUp && vy > 0f)
            vy = 0f;
        
        if (!moveDown && vy < 0f)
            vy = 0f;


        if (!moveLeft && vx < 0f)
            vx = 0f;
        
        if (!moveRight && vx > 0f)
            vx = 0f;


        GetComponent<Rigidbody>().velocity = new Vector3(vx, vy, 0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
            grounded = true;
    }
}