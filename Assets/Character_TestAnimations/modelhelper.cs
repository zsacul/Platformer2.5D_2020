using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelhelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0f, -1.8f, 0f);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
    }
}
