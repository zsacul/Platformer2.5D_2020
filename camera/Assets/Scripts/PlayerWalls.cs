using UnityEngine;

public class PlayerWalls : MonoBehaviour
{
    public KeyCode up, left, right;
    bool grounded;
    float horizontalSpeed = 4f;
    float jumpSpeed = 6f;

    void Start() 
    {
        grounded = true;
    }

    void Update()
    {
        float vy = GetComponent<Rigidbody>().velocity.y;

        if (Input.GetKeyDown(up) && grounded)
        {
            vy = jumpSpeed;
            grounded = false;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(0f, vy, 0f);

        if (Input.GetKey(left))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-horizontalSpeed, vy, 0f);
        }
        
        if (Input.GetKey(right))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(horizontalSpeed, vy, 0f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
            grounded = true;
    }
}