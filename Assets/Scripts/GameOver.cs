using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    public GameObject text;
    public GameObject panel;
    public GameObject reflector;
    public LoopedSoundAttachment helicopterSound;
    public GameObject credits;


    public void OnTriggerEnter(Collider other)
    {
        reflector.SetActive(false);
        helicopterSound.Stop();
        //Debug.Log("Game Over");
        text.SetActive(true);
        panel.SetActive(true);
        text.GetComponent<Animation>().Play();
        panel.GetComponent<Animation>().Play();
        StartCoroutine(WaitForCredits(7f));
    }

    public IEnumerator WaitForCredits(float sec)
    {
        yield return new WaitForSeconds(sec);
        credits.SetActive(true);
        credits.GetComponent<Animation>().Play();
        StartCoroutine(WaitForEnd(21f));
    }

    public IEnumerator WaitForEnd(float sec)
    {
        yield return new WaitForSeconds(sec);
        Application.Quit();
        Debug.Log("Game just ended");

    }
}
