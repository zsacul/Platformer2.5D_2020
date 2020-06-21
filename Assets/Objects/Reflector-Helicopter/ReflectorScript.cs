using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorScript : MonoBehaviour
{
    public float speed = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
        //speed -= 0.1f * Time.deltaTime;
    }

 
}
