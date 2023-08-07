using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(SpeedrunManager.instance.levelTime);

        if (time.Hours > 0)
        {
            text.text = time.ToString("h':'mm':'ss'.'ff");
        }
        else
        {
            text.text = time.ToString("m':'ss'.'ff");
        }
    }
}
