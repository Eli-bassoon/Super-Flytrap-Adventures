using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager MM;
    [HideInInspector] public AudioSource audioSource;

    // Start is called before the first frame update

    private void Awake()
    {
        if (MM == null) MM = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void SetMusicTrack(AudioClip track)
    {
        audioSource.clip = track;
        audioSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private string GetMusicFilepath(string trackName)
    {
        return "Assets/Music/" + trackName + ".mp3";
    } 
}
