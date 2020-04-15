using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    ///singleton, use this to acces anything (I short for Instance)
    [HideInInspector]
    public static AudioManager Instance;

    ///container for audio clips and mixers
    [HideInInspector]
    public AudioAssets Sounds;

    [SerializeField]
    AnimationCurve defaultFadingCurve;


    private AudioSource src; 

    ///does not interfere in ANY way with any sound, even on the same object
    public void PlaySoundOnce(AudioClip soundToPlay, GameObject originOfSound, AudioMixerGroup mixer)
    {
        if(originOfSound.GetComponent<AudioSource>() == null)
        {
            AudioSource newSource = originOfSound.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.spatialBlend = 1.0f;
        }
       
        AudioSource src = originOfSound.GetComponent<AudioSource>();
        src.outputAudioMixerGroup = mixer;
        src.PlayOneShot(soundToPlay);
    }

    ///duration 0 means it will play till manually stopped 
    public void PlayLooped(AudioClip soundToPlay, GameObject originOfSound, AudioMixerGroup mixer, float duration = 0.0f)
    {

    }

    ///this only works if some clip is already being played on the source. Also, it stops audioSource after the duration and restores original volume
    public void FadeOut(GameObject source, float duration, AnimationCurve curve = null)
    {
        AnimationCurve Curve = curve;
        if (Curve == null)
        {
            Curve = defaultFadingCurve;
        }

        AudioSource aSrc = source.GetComponent<AudioSource>();

        IEnumerator IEnumForFade = FadeOutCoroutine(aSrc, duration, Curve, aSrc.volume);
        StartCoroutine(IEnumForFade);
    }
    
    ///this only works if some clip is already being played on the source
    public void FadeIn(GameObject source, float duration, AnimationCurve curve = null)
    {
        AnimationCurve Curve = curve;
        if (Curve == null)
        {
            Curve = defaultFadingCurve;
        }

        AudioSource aSrc = source.GetComponent<AudioSource>();

        IEnumerator IEnumForFade = FadeInCoroutine(aSrc, duration, Curve, aSrc.volume);
        StartCoroutine(IEnumForFade);
    }

    //fades out(if specified) current bgm and fades in the new one
    public void PlayBGM(AudioClip BGM, float fadeInDuration = 1.0f, float fadeOutDuration = 1.0f)
    {
        
        if(src.isPlaying)
        {
            IEnumerator enumerator = ChangeBGM(BGM, fadeInDuration, fadeOutDuration);
            StartCoroutine(enumerator);
        }
        else
        {
            src.clip = BGM;
            src.Play();
            FadeIn(this.gameObject, fadeInDuration);
        }
    }

    //TODO maybe
    public void AddBgmToQueue()
    {

    }

    //TODO maybe
    public void PauseBGM(float fadeOutDuration = 1.0f)
    {

    }
        

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration, AnimationCurve curve, float startingVol)
    {
        float currtime = 0;

        while(currtime<duration)
        {
            source.volume = Mathf.Lerp(startingVol, 0.0f, curve.Evaluate(currtime / duration));
            currtime += Time.deltaTime;
            yield return null;
        }
        
       source.Stop();
       source.volume = startingVol;
    }
    private IEnumerator FadeInCoroutine(AudioSource source, float duration, AnimationCurve curve, float targetVol)
    {
        float currtime = 0;

        while (currtime < duration)
        {
            source.volume = Mathf.Lerp(0.0f, targetVol, curve.Evaluate(currtime / duration));
            currtime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ChangeBGM(AudioClip nextBGMtoPlay,  float fadeInDuration, float fadeOutDuration)
    {
        float currtime = 0;
        
        float startingVol = src.volume;
        while (currtime < fadeOutDuration)
        {
            src.volume = Mathf.Lerp(startingVol, 0.0f, defaultFadingCurve.Evaluate(currtime / fadeOutDuration));
            currtime += Time.deltaTime;
            yield return null;
        }

        src.Stop();
        src.volume = startingVol;
        src.clip = nextBGMtoPlay;
        src.Play();
        FadeIn(this.gameObject, fadeInDuration);
    }

    void Start()
    {
       src.clip = Sounds.sBGMAction1;
       src.Play();
       FadeOut(this.gameObject, 8.0f);
    }
    private void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance);
        else
            Instance = this;

        DontDestroyOnLoad(this);

        Sounds = GetComponent<AudioAssets>();
        src = this.gameObject.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = Sounds.mBGMMaster;
        src.spatialBlend = 0;
        src.maxDistance = 9999999;
    }

    //just for testing
    float f = 0;
    void Update()
    {
        f += Time.deltaTime;
        if (f > 11)
        {
            PlayBGM(Sounds.sBGMAmbient3, 3.0f, 5.0f);
            f = 0;
        }
           
    
    }


}
