using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 lastPos;

    void Awake()
    {
        lastPos = transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Checkpoint"))
        {
            Debug.Log("zapisuje checkpoint");
            lastPos = new Vector3(col.gameObject.transform.position.x, col.gameObject.transform.position.y, lastPos.z);
            Destroy(col.gameObject);
        }
    }

    
}
