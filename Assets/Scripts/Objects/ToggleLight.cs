using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleLight : Toggleable
{
    private Animator anim;
    private Light2D lamp;

    void Start()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();
        anim.SetBool("activated", on);
        lamp.enabled = on;
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);
        anim.SetBool("activated", on);
        lamp.enabled = on;
    }

    protected override void SetOnEditor()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();
        SetOn(on);
    }
}
