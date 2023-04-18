using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Toggleable: MonoBehaviour, IBoolAcceptor
{
    [OnValueChanged("SetOnEditor")][SerializeField] protected bool on = false;
    [SerializeField] protected bool invertReceivedBool = false;

    protected virtual void Start()
    {
        SetOn(on);
    }

    public virtual void SetOn(bool state)
    {
        on = state;
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

    public void TakeBool(bool value)
    {
        // We XOR the received value and whether we invert it to get the new on value
        SetOn(value ^ invertReceivedBool);
    }

    public bool IsOn() => on;

    protected virtual void SetOnEditor()
    {
        SetOn(on);
    }
}
