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
    public AudioClip sTowerShotBlaster;
 
    //background music
    public AudioClip sBGMAction1;
    public AudioClip sBGMAction2;
    public AudioClip sBGMAction3;
    public AudioClip sBGMAmbient1;
    public AudioClip sBGMAmbient2;
    public AudioClip sBGMAmbient3;

    //mixers
    public AudioMixerGroup mProjectilesMaster;
    public AudioMixerGroup mBGMMaster;
}
