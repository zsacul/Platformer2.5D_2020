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

    [SerializeField]
    GameObject sourceTemplate;

    private AudioSource src;

    [SerializeField]
    float BGMFadeOutDuration = 5.0f;
    [SerializeField]
    float BGMFadeInDuration = 1.0f;


    /// do not use this!!
    [HideInInspector]
    public Queue<GameObject> freeSources = new Queue<GameObject>();

    private Queue<AudioClip> BGMqueue = new Queue<AudioClip>();
    private List<AudioClip> ambientMusic = new List<AudioClip>();
    private List<AudioClip> actionMusic = new List<AudioClip>();

    [SerializeField]
    bool shouldUseBgmQueue = true;

    private bool isBGMFadingOut = false;


    ///does not interfere in ANY way with any sound, even on the same object
    public void PlaySoundOnce(AudioClip soundToPlay, GameObject originOfSound, AudioMixerGroup mixer)
    {
        if (originOfSound.GetComponent<AudioSource>() == null)
        {
            AudioSource newSource = originOfSound.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.enabled = true;
            newSource.spatialBlend = 1.0f;
        }

        AudioSource src = originOfSound.GetComponent<AudioSource>();
        src.outputAudioMixerGroup = mixer;
        src.PlayOneShot(soundToPlay);
    }

    ///duration 0 means it will play till manually stopped 
    public void PlayLooped(AudioClip soundToPlay, GameObject originOfSound, AudioMixerGroup mixer, float duration = 0.0f)
    {
        if (originOfSound.GetComponent<AudioSource>() == null)
        {
            AudioSource newSource = originOfSound.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.spatialBlend = 1.0f;
        }
        AudioSource src = originOfSound.GetComponent<AudioSource>();
        src.outputAudioMixerGroup = mixer;
        src.loop = true;
        src.clip = soundToPlay;
        src.Play();
    }

    public void StopPlaying(GameObject originOfSound)
    {
        originOfSound.GetComponent<AudioSource>().Stop();
    }

    ///this only works if some clip is already being played on the source. Also, it stops audioSource after the duration and restores original volume
    public void FadeOut(GameObject source, float duration, AnimationCurve curve = null)
    {
        if (source == null)
            return;
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
        if (source == null)
            return;
        AnimationCurve Curve = curve;
        if (Curve == null)
        {
            Curve = defaultFadingCurve;
        }

        AudioSource aSrc = source.GetComponent<AudioSource>();

        IEnumerator IEnumForFade = FadeInCoroutine(aSrc, duration, Curve, aSrc.volume);
        StartCoroutine(IEnumForFade);
    }

    ///fades out(if specified) current bgm and fades in the new one
    ///can be used even if BGM quque is in use, it will just replace 
    ///the current BGM unless it's fading out, and after
    ///it's finished the next BGM from queue shall be played
    public void PlayBGM(AudioClip BGM, float fadeInDuration = 1.0f, float fadeOutDuration = 1.0f)
    {

        if (src.isPlaying)
        {
            if (!isBGMFadingOut)
            {
                IEnumerator enumerator = ChangeBGM(BGM, fadeInDuration, fadeOutDuration);
                StartCoroutine(enumerator);
            }
        }
        else
        {
            src.clip = BGM;
            src.Play();
            FadeIn(this.gameObject, fadeInDuration);
        }
    }

    ///use this to play sounds on dynamic objects that are disappearing or appearing once, like bullet hits or some particle effects
    public void PlayOnceAtLocation(AudioClip soundToPlay, Vector3 originOfSound, AudioMixerGroup mixer = null)
    {
        GameObject srcLoc = null;
        AudioSource src;


        while(srcLoc == null)
        {
            if (freeSources.Count > 0)
                srcLoc = freeSources.Dequeue();
            else
                srcLoc = Instantiate(sourceTemplate, originOfSound, sourceTemplate.transform.rotation);
        }
        

        srcLoc.SetActive(true);
        srcLoc.transform.position = originOfSound;
        src = srcLoc.GetComponent<AudioSource>();
        if (mixer != null)
            src.outputAudioMixerGroup = mixer;
        src.clip = soundToPlay;
        src.Play();
    }


    public void AddBgmToQueue(AudioClip BGMtoAdd)
    {
        BGMqueue.Enqueue(BGMtoAdd);
    }

    public void PauseBGM(float fadeOutDuration = 1.0f)
    {
        src.Pause();
    }

    public void ResumeBGM()
    {
        src.UnPause();
    }

    public void PlayActionBGM()
    {
        shouldUseBgmQueue = true;
        BGMqueue.Clear();

        for (int i = 0; i < 10; i++)
        {
            int r = Random.Range(0, actionMusic.Count);
            BGMqueue.Enqueue(actionMusic[r]);
        }
        PlayBGM(BGMqueue.Dequeue(), BGMFadeInDuration, BGMFadeOutDuration + 2);
    }

    public void PlayAmbientBGM()
    {
        shouldUseBgmQueue = true;
        BGMqueue.Clear();

        for (int i = 0; i < 10; i++)
        {
            int r = Random.Range(0, ambientMusic.Count);
            BGMqueue.Enqueue(ambientMusic[r]);
        }
        PlayBGM(BGMqueue.Dequeue(), BGMFadeInDuration + 4, BGMFadeOutDuration);
    }

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration, AnimationCurve curve, float startingVol)
    {
        float currtime = 0;

        while (currtime < duration)
        {
            if (source == null)
                yield break;
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
            if (source == null)
                yield break;
            source.volume = Mathf.Lerp(0.0f, targetVol, curve.Evaluate(currtime / duration));
            currtime += Time.deltaTime;
            yield return null;
        }
    }


    private IEnumerator ChangeBGM(AudioClip nextBGMtoPlay, float fadeInDuration, float fadeOutDuration)
    {
        float currtime = 0;
        isBGMFadingOut = true;
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
        isBGMFadingOut = false;
    }

    void Start()
    {

        ambientMusic.Add(Sounds.sBGMAmbient1);
        ambientMusic.Add(Sounds.sBGMAmbient2);
        ambientMusic.Add(Sounds.sBGMAmbient3);
        //ambientMusic.Add(Sounds.sBGMAmbient4);

        //actionMusic.Add(Sounds.sBGMAction1);
        //actionMusic.Add(Sounds.sBGMAction2);
        //actionMusic.Add(Sounds.sBGMAction3);
        //actionMusic.Add(Sounds.sBGMAction4);
        AddBgmToQueue(Sounds.sBGMAmbient1);
        if (shouldUseBgmQueue)
        {
            PlayBGM(Sounds.sBGMAmbient1);
        }
        /* if (shouldUseBgmQueue)
        {
            PlayAmbientBGM();
        }*/
    }
    private void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance);
        else
            Instance = this;

        //DontDestroyOnLoad(this);

        Sounds = GetComponent<AudioAssets>();
        src = this.gameObject.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = Sounds.mBGM;
        src.spatialBlend = 0;
        src.maxDistance = 9999999;
       
    }

    //just for testing
    float f = 0;
    void Update()
    {
        //Debug.Log("currentTime: " + (src.clip.length - src.time - BGMFadeInDuration).ToString());
        if (shouldUseBgmQueue && src.isPlaying)
        {
            if (src.time + BGMFadeOutDuration >= src.clip.length)
            {
                if (BGMqueue.Count > 0)
                    PlayBGM(BGMqueue.Dequeue(), BGMFadeInDuration, BGMFadeOutDuration);
                else
                    PlayBGM(src.clip, BGMFadeInDuration, BGMFadeOutDuration);
            }
        }


    }


}
