using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSoundAttachment : BaseSoundAttachment
{
    override public void Play()
    { 

        //AudioManager.Instance.PlaySoundOnce(clipToPlay, this.gameObject, mixer);

        if (clipToPlay!=null && mixer!=null && AudioManager.Instance!=null)
            AudioManager.Instance.PlaySoundOnce(clipToPlay, this.gameObject, mixer);
    }
}
