﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float lifeTime = 3f;
    float startTime;
    public bool power = false;

    void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > lifeTime)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {	
		if(!col.gameObject.CompareTag("Player1") && !col.gameObject.CompareTag("Bullet"))
			Destroy(this.gameObject);
    }
}
