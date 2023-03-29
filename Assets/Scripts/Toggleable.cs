using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable: MonoBehaviour
{
    [OnValueChanged("SetOn_")][SerializeField] protected bool on = false;

    private void Start()
    {
        SetOn(on);
    }

    public void TurnOn()
    {
        SetOn(true);
    }

    public void TurnOff()
    {
        SetOn(false);
    }

    public void Toggle()
    {
        SetOn(!on);
    }

    public virtual void SetOn(bool state)
    {
        on = state;
    }

    public bool IsOn() => on;

    private void SetOn_()
    {
        SetOn(on);
    }
}
