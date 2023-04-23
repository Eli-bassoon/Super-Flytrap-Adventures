using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource[] audioSources;

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

    public void PlaySound(AudioClip clip)
    {
        //_effectsSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ToggleEffects()
    {
        //_effectsSource.mute = !_effectsSource.mute;
    }


    public void ChangeVolume(float value)
    {

    }



}
