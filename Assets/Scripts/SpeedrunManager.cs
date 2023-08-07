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

        levelTimes = new List<float>(new float[2]);
    }

    public void StopLevel(int level)
    {
        levelTimes[level] = levelTime;
        savedTime = levelTime;
        running = false;
    }

    public void StartLevel(int level)
    {
        running = true;
        levelStartTime = Time.time;
    }
}
