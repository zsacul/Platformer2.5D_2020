using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSoundAttachment : BaseSoundAttachment
{
    override public void Play() 
    {
         AudioManager.Instance.PlaySoundOnce(clipToPlay, this.gameObject, mixer);
        // AudioManager.Instance.fadeOut(this.gameObject, 1.1f);
        //AudioManager.Instance.PlayOnceAtLocation(clipToPlay, this.transform.position);
    }
}
