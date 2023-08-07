using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalTimesText : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void ShowTimes()
    {
        SpeedrunManager.instance.StopLevel();

        text.text = "<color=#00CE00>Times:</color>";

        for (int i = 0; i < GameManager.NUM_LEVELS; i++)
        {
            text.text += $"\n<color=#FFFF00>Level {i+1}</color><color=#FF0000> - </color>";
            text.text += SpeedrunManager.FormatTime(SpeedrunManager.instance.levelTimes[i]);
        };
    }
}
