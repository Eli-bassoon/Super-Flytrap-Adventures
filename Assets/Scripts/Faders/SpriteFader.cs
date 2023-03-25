using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFader : Fader
{
    SpriteRenderer sr;
    Image img;

    protected override void Awake()
    {
        TryGetComponent(out sr);
        TryGetComponent(out img);
    }

    protected override float InitStartValue()
    {
        if (sr != null)
            return sr.color.a;
        else if (img != null)
            return img.color.a;
        else
            throw new System.NullReferenceException();
    }

    protected override void SetValue(float value)
    {
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
        else if (img != null)
            img.color = new Color(img.color.r, img.color.g, img.color.b, value);
    }
}
