using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleLight : Toggleable
{
    [SerializeField] AudioClip lightSound;

    private Animator anim;
    private Light2D lamp;

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();

        base.Start();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);
        anim.SetBool("activated", on);
        lamp.enabled = on;

        if (Vector2.Distance(transform.position, PlayerMovement.instance.transform.position) < 15f)
            SoundManager.SM.PlaySound(lightSound);
    }

    protected override void SetOnEditor()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();
        SetOn(on);
    }
}
