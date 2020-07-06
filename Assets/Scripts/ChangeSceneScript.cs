using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneScript : MonoBehaviour
{
    private int nextScene;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GetComponentInChildren<Animator>().Play("screenFadeOut");
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    IEnumerator fadeRoutine(int nextScene)
    {
        GetComponentInChildren<Animator>().Play("screenFadeIn");
        yield return new WaitForSeconds(0.22f);
        SceneManager.LoadScene(nextScene);
    }

    private void OnCollisionEnter(Collision collision)
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(fadeRoutine(nextScene));
        
    }
}
