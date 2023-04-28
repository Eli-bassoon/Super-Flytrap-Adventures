using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager MM;
    [SerializeField] AudioSource a;

    // Start is called before the first frame update

    private void Awake()
    {
        if (MM == null) MM = this;
    }
    void Start()
    {
        // a = GetComponent<AudioSource>();
    }

    public void SetMusicTrack(AudioClip track)
    {
        a.clip = track;
    }
    public void SetMusicVolume(float volume)
    {
        a.volume = volume;
    }



    private string GetMusicFilepath(string trackName)
    {
        return "Assets/Music/" + trackName + ".mp3";
    } 
}
