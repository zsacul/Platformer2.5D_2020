﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1SoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void RunningStep()
    {
        AudioManager.Instance.PlaySoundOnce(AudioManager.Instance.Sounds.sRunning, this.gameObject, AudioManager.Instance.Sounds.mAnimations);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}