using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ChangeSceneScript : MonoBehaviour
{
    public UnityEvent levelUp;
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }

    private void OnCollisionEnter(Collision collision)
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(fadeRoutine(nextScene));
        if (levelUp != null)
            levelUp.Invoke();
        
    }
}
