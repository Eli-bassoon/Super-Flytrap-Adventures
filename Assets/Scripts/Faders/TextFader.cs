using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFader : Fader
{
    TextMeshProUGUI text;

    protected override void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    protected override float InitStartValue()
    {
        return text.color.a;
    }

    protected override void SetValue(float value)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, value);
    }
}
