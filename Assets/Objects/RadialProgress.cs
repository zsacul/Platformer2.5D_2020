using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    public Image Bar;
    public float duration;
    float currentTime;
    
    void Start() {}

    void Update()
    {
        if (currentTime < duration)
            currentTime += Time.deltaTime;
            
        Bar.fillAmount = currentTime / duration;
    }
}
