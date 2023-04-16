using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] tracks;

    [SerializeField] int trackNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        MusicManager.MM.SetMusicTrack(tracks[trackNum]);
    }
}
