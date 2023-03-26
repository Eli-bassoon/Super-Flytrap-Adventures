using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFader : Fader
{
    Light2D light2D;

    protected override void Awake()
    {
        light2D = GetComponent<Light2D>();
    }

    protected override float InitStartValue()
    {
        return light2D.intensity;
    }

    protected override void SetValue(float value)
    {
        light2D.intensity = value;
    }
}
