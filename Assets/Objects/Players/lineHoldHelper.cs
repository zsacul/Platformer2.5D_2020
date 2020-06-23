using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class lineHoldHelper : MonoBehaviour
{
    [HideInInspector]
    public bool canCatch;
    [HideInInspector]
    public GameObject currentLinePart;

    public List<GameObject> lineParts;    

    public Vector3 PickBestFitting()
    {
        float bestDist = 1000.0f;
        Vector3 ret = new Vector3();

        foreach(GameObject part in lineParts)
        {
            if(Vector3.Distance(transform.position, part.transform.position) < bestDist)
            {
                bestDist = Vector3.Distance(transform.position, part.transform.position);
                ret = part.transform.position;
            }
        }
        return ret;
    }
}
