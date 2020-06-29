using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundsHandler : MonoBehaviour
{
    public void EnemyStep()
    {
        AudioManager.Instance.PlaySoundOnce(AudioManager.Instance.Sounds.sRunning, this.gameObject, AudioManager.Instance.Sounds.mAnimations);
    }

    public void EnemyClimb()
    {
        AudioManager.Instance.PlaySoundOnce(AudioManager.Instance.Sounds.sCrouchWalking, this.gameObject, AudioManager.Instance.Sounds.mAnimations);
    }
}
