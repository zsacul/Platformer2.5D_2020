using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class startScreen : MonoBehaviour
{
    protected int nextScene;
    void Start()
    {
        StartCoroutine(Wait());
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(8f);
        //LoadFirstScene
        SceneManager.LoadScene(nextScene);
    }
}
