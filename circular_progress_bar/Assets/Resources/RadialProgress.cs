using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    public Image Bar;
    public float duration;
    float currentTime;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < duration)
            currentTime += Time.deltaTime;
        Bar.fillAmount = currentTime / duration;
    }
}
