using UnityEngine;

public class Walls : MonoBehaviour
{
    public bool horizontal;
    void Start() {}

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag != "player_1" && collision.gameObject.tag != "player_2") 
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        else
        {
            Vector3 v = collision.collider.GetComponent<Rigidbody>().velocity;
            
            if (horizontal)
                collision.collider.GetComponent<Rigidbody>().velocity = new Vector3(0f, v.y, 0f);
            else
                collision.collider.GetComponent<Rigidbody>().velocity = new Vector3(v.x, 0f, 0f);
        }
    }
 
}