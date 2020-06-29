using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScript : MonoBehaviour
{
    // Start is called before the first frame update
    int counter  = 0;
    public void printSomeShit()
    {
        counter += 1;
        Debug.Log("someShit: " + counter);
    }
}
