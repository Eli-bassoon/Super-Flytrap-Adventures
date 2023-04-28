using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource[] audioSources;

    public static SoundManager SM;

    void Awake()
    {
        if (SM == null)
        {
            SM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    public AudioSource PlaySound(AudioClip clip)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = clip;
                audioSource.Play();
                return audioSource;
            }
        }
        return null;
    }

    public void ChangeVolume(float value)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = value;
        }
    }

    public void ToggleEffects()
    {
        //_effectsSource.mute = !_effectsSource.mute;
    }
}
