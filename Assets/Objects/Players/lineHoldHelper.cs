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

    public int PickBestFitting()
    {
        float bestDist = 1000.0f;
        int ret = 0, it = 0;
        foreach(GameObject part in lineParts)
        {
            if(Vector3.Distance(transform.position, part.transform.position) < bestDist)
            {
                bestDist = Vector3.Distance(transform.position, part.transform.position);
                ret = it;
            }
            it += 1;
        }
        return ret;
    }
}
