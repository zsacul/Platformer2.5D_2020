﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1SoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void RunningStep()
    {
        AudioManager.Instance.PlaySoundOnce(AudioManager.Instance.Sounds.sRunning, this.gameObject, AudioManager.Instance.Sounds.mAnimations);
    }

    public void ClimbSound()
    {
        AudioManager.Instance.PlaySoundOnce(AudioManager.Instance.Sounds.sClimbing, this.gameObject, AudioManager.Instance.Sounds.mAnimations);
    }

}
