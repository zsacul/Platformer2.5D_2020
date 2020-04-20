using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class BaseSoundAttachment : MonoBehaviour
{
    [SerializeField]
    protected AudioClip clipToPlay;

    [SerializeField]
    protected AudioMixerGroup mixer;

    public abstract void Play();
 
}
