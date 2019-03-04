using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    protected static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoundManager)FindObjectOfType(typeof(SoundManager));

                if (instance == null)
                {
                    Debug.LogWarning("SoundManager Singleton Error");
                }
            }

            return instance;
        }
    }

    // === AudioSource ===
    // BGM
    private AudioSource BGMsource;

    private AudioSource MoveSource;
    // SE
    private AudioSource[] SEsources = new AudioSource[24];
    // Voice
    private AudioSource[] VoiceSources = new AudioSource[4];

    // === AudioClip ===
    // Move
    public AudioClip Move;

    // BGM
    public AudioClip[] BGM;
    // SE
    public AudioClip[] SE;
    // Voice
    public AudioClip[] Voice;

    // === Status ===
    bool isFadingIn;
    bool isFadingOut;
    public float fadeSpeed = 1.25f;

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
        if (obj.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        BGMsource = gameObject.AddComponent<AudioSource>();
        BGMsource.loop = true;

        MoveSource = gameObject.AddComponent<AudioSource>();
        MoveSource.loop = true;

        // SE AudioSource
        for (int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

        // Voice AudioSource
        for (int i = 0; i < VoiceSources.Length; i++)
        {
            VoiceSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayMoving(bool state)
    {
        if(state)
        {
            MoveSource.clip = Move;
            MoveSource.Play();
        }
        else
        {
            MoveSource.Stop();
        }
    }

    public void PlayIntro()
    {
        PlayBGM(0);
    }

    public void PlayStageBGM()
    {
        PlayBGM(1);
    }

    public void PlayBattleBGM()
    {
        PlayBGM(2);
    }

    public void PlayEnding()
    {
        PlayBGM(3);
    }

    private IEnumerator FadeInRoutine(int index)
    {
        isFadingIn = true;
        BGMsource.clip = BGM[index];
        BGMsource.volume = 0f;
        BGMsource.Play();

        while (BGMsource.volume < 1.0f)
        {
            BGMsource.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        BGMsource.volume = 1.0f;

        isFadingIn = false;
    }

    private IEnumerator FadeOutRoutine()
    {
        if(BGMsource.clip != null)
        {
            isFadingOut = true;

            while (BGMsource.volume > 0.0f)
            {
                BGMsource.volume -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            BGMsource.volume = 0.0f;
            BGMsource.Stop();
            BGMsource.clip = null;

            isFadingOut = false;
        }
    }

    // BGM
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index)
        {
            return;
        }

        if (BGMsource.clip == BGM[index])
        {
            return;
        }

        StartCoroutine(PlayBGMRoutine(index));
    }

    IEnumerator PlayBGMRoutine(int index)
    {
        yield return FadeOutRoutine();
        yield return FadeInRoutine(index);
    }

    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    // SE
    public void PlaySE(int index)
    {
        if (0 > index || SE.Length <= index)
        {
            return;
        }

        // Find empty AudioSource and play it
        foreach (AudioSource source in SEsources)
        {
            if (!source.isPlaying)
            {
                source.clip = SE[index];
                source.Play();
                return;
            }
        }
    }

    public void StopSE()
    {
        // Stop all AudioSource
        foreach (AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


    // Voice
    public void PlayVoice(int index)
    {
        if (0 > index || Voice.Length <= index)
        {
            return;
        }
        // // Find empty AudioSource and play it
        foreach (AudioSource source in VoiceSources)
        {
            if (!source.isPlaying)
            {
                source.clip = Voice[index];
                source.Play();
                return;
            }
        }
    }

    public void StopVoice()
    {
        // Stop all AudioSource
        foreach (AudioSource source in VoiceSources)
        {
            source.Stop();
            source.clip = null;
        }
    }
}