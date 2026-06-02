using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;
    public AudioSource musicSource0,musicSource1;
    private int musicSourceIndex = 0;
    private Coroutine musicTransition;
    
    [SerializedDictionary("nom","AudioClip")]
    public SerializedDictionary<string, AudioClip> sfx = new SerializedDictionary<string, AudioClip>();
    
    [SerializedDictionary("nom","AudioClip")]
    public SerializedDictionary<string, AudioClip> soundLoops= new SerializedDictionary<string, AudioClip>();
    
    public AudioSource[] loopSources = new AudioSource[3];
    public bool[] loopBools = new bool[3];
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(string name, float pitchRandomness = 0f)
    {
        if (!Instance)
        {
            Debug.LogWarning("SoundManager missing");
            return;
        }
        Instance.audioSource.pitch = 1 + (Random.Range(-pitchRandomness,pitchRandomness));
        Instance.audioSource.PlayOneShot(Instance.sfx[name]);
    }

    public static void PlayMusic(AudioClip clip)
    {
        if (!Instance)
        {
            Debug.LogWarning("SoundManager missing");
            return;
        }
        if (Instance.musicTransition == null)
            Instance.musicTransition = Instance.StartCoroutine(Instance.MusicTransition(clip));
        else 
        { 
            Instance.StopCoroutine(Instance.musicTransition); 
            Instance.musicTransition = Instance.StartCoroutine(Instance.MusicTransition(clip));
        }
    }

    public static int? CreateLoopSound(AudioClip clip)
    {
        if (!Instance)
        {
            Debug.LogWarning("SoundManager missing");
        }

        for (int i = 0; i < 3; i++)
        {
            if (!Instance.loopBools[i])
            {
                Instance.loopBools[i] = true;
                Instance.loopSources[i].clip = clip;
                Instance.loopSources[i].Play();
                return i;
            }
        }
        Debug.LogWarning("no loops available");
        return null;
    }

    public static void StopLoop(int i)
    {
        Instance.loopBools[i] = false;
        Instance.loopSources[i].Stop();
    }

    public IEnumerator MusicTransition(AudioClip newMusic, float time =2f)
    {
        AudioSource currentSource = musicSource0;
        AudioSource otherSource = musicSource1;
        if ( musicSourceIndex == 1) { currentSource =  musicSource1; otherSource = musicSource1; }

        float counter = 0f;
        otherSource.clip = newMusic;
        otherSource.Play();
        while (counter < time)
        {
            counter+=Time.deltaTime;
            currentSource.volume = 1-(counter/time);
            otherSource.volume = counter/time;
            yield return null;
        }
        currentSource.Stop();
    }
}
