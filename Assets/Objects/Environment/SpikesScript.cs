﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;

public class SpikesScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player1") && other.gameObject.GetComponent<Rigidbody>())
		{
			other.gameObject.GetComponent<PlayerScript>().Cought();
		}
		if(other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<EnemyScript>().Death();
		}
		
	}
}
