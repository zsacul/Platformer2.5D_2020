using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Wait());
        //LoadFirstScene
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(8f);
        //LoadFirstScene
    }
}
