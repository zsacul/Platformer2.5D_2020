using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIsound2d : ShortSoundAttachment
{
    // Start is called before the first frame update
    AudioSource src;
    void Start()
    {
        if(this.GetComponent<AudioSource>() == null)
        {
            this.gameObject.AddComponent<AudioSource>();            
        }
        src = GetComponent<AudioSource>();     
        src.spatialBlend = 0.0f;
    }

}
