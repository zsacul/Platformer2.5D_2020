using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceLoc : MonoBehaviour
{
    AudioSource src;
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!src.isPlaying)
        {
            AudioManager.Instance.freeSources.Enqueue(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
