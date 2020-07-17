﻿using System;
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
    public GameObject credits;


    public void OnTriggerEnter(Collider other)
    {
        reflector.SetActive(false);
        //Debug.Log("Game Over");
        text.GetComponent<Animation>().Play();
        panel.GetComponent<Animation>().Play();
        StartCoroutine(WaitForCredits(7f));
    }

    public IEnumerator WaitForCredits(float sec)
    {
        yield return new WaitForSeconds(sec);
        credits.GetComponent<Animation>().Play();
    }
}
