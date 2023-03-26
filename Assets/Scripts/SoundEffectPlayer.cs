using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
    }

    public void Play()
    {
        audioSource.PlayOneShot(audioSource.clip, audioSource.volume);
    }
}