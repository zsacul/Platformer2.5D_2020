using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifeTime = 3f;

    float startTime;
    void Awake()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > lifeTime)
            Destroy(this.gameObject);
    }

    /*void OnCollisionEnter()
    {

        Destroy(this.gameObject);
    }*/
}
