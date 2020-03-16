using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public Transform playerPos;

    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector3(playerPos.position.x, transform.position.y, transform.position.z);
    }
}
