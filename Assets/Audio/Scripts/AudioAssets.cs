using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*This is just a container for sounds and mixers
used only if one needs to acces some sound asset from code.
AudioClips start with s, AudioMixers start with m, please follow this rule.
*/


public class AudioAssets : MonoBehaviour
{
    //sfx
    public AudioClip sRunning;
    public AudioClip sWalking;
    public AudioClip sCrouchWalking;

    //background music
    public AudioClip sBGM1;
    public AudioClip sBGM2;
    public AudioClip sBGM3;
    public AudioClip sBGMAmbient1;
    public AudioClip sBGMAmbient2;
    public AudioClip sBGMAmbient3;

    //mixers
    public AudioMixerGroup mMASTER;
    public AudioMixerGroup mSFX;
    public AudioMixerGroup mBGM;
    public AudioMixerGroup mAnimations;
}
