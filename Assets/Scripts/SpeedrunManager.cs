using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunManager : MonoBehaviour
{
    public static SpeedrunManager instance;

    public List<float> levelTimes;

    bool running = false;

    float levelStartTime;
    float savedTime;

    // The elapsed time in the current level
    public float levelTime
    {
        get {
            if (running)
                return Time.time - levelStartTime;
            else return savedTime;
        }
    }

    private void Awake()
    {
        instance = this;

        ResetTimes();
    }

    public void StopLevel()
    {
        levelTimes[GameManager.GM.level] = levelTime;
        savedTime = levelTime;
        running = false;
    }

    public void StartLevel()
    {
        running = true;
        levelStartTime = Time.time;
    }

    public void ResetTimes()
    {
        running = false;
        savedTime = 0;
        levelTimes = new List<float>(new float[GameManager.NUM_LEVELS]);
    }

    // Turns a time in seconds into a string depending on how long it took
    public static string FormatTime(float timeSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeSeconds);

        if (time.Hours > 0)
        {
            return time.ToString("h':'mm':'ss'.'ff");
        }
        else
        {
            return time.ToString("m':'ss'.'ff");
        }
    }
}
