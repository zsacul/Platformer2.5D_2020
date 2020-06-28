using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReflectorScript : MonoBehaviour
{
    public UnityEvent helicopterFlying;
    public float speed = 3f;
    void Start()
    {
        if(helicopterFlying != null)
        {
            helicopterFlying.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
        //speed -= 0.1f * Time.deltaTime;
    }

 
}
