using System.Collections;
using UnityEngine;

public class EnvironmentElement : MonoBehaviour
{
    static int elementsCounter = 0;
    int id;

    public int GetId ()
    {
        return id;
    }

    protected EnvironmentElement ()
    {
        id = elementsCounter++;
    }
}