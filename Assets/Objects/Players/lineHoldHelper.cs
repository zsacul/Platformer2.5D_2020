using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineHoldHelper : MonoBehaviour
{
    [HideInInspector]
    public bool canCatch;
    [HideInInspector]
    public GameObject currentLinePart;

    void Update()
    {
        Debug.Log(canCatch);
    }

    void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("col");
        if (other.gameObject.tag == "line")
        {
            if (currentLinePart == null || currentLinePart == other.gameObject || (currentLinePart.transform.position.y <= other.gameObject.transform.position.y && Input.GetKey(KeyCode.UpArrow)))
            {
                currentLinePart = other.gameObject;
                canCatch = true;
            }
            else
            {
                canCatch = false;
            }
        }
        else
        {
            canCatch = false;
            currentLinePart = null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        canCatch = false;
        currentLinePart = null;
    }
}
