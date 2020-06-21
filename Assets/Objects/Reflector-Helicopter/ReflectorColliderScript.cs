using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectorColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            other.gameObject.GetComponent<PlayerScript>().Cought();
        }
    }
}
