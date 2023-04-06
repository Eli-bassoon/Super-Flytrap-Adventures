using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightActivated : LightDetector
{
    [Tooltip("Whether the object is activated (true) or deactivated (false) by light")]
    [SerializeField] protected bool activateOnShine = true;

    protected override void OnShineStart()
    {
        if (activateOnShine) OnActivate();
        else OnDeactivate();
    }

    protected override void OnShineEnd()
    {
        if (activateOnShine) OnDeactivate();
        else OnActivate();
    }

    protected virtual void OnActivate() { }

    protected virtual void OnDeactivate() { }
}
