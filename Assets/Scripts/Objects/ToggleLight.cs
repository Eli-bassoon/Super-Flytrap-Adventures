using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ToggleLight : MonoBehaviour
{
    private Animator anim;
    private Light2D lamp;
    [OnValueChanged("UpdateLightState")] public bool activated = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();
        anim.SetBool("activated", activated);
        lamp.enabled = activated;
    }

    public void TurnOn()
    {
        SetLightState(true);
    }

    public void TurnOff()
    {
        SetLightState(false);
    }

    public void SetLightState(bool on)
    {
        activated = on;
        anim.SetBool("activated", activated);
        lamp.enabled = activated;
    }

    private void UpdateLightState()
    {
        anim = GetComponent<Animator>();
        lamp = GetComponentInChildren<Light2D>();
        SetLightState(activated);
    }
}
