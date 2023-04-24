using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Toggles all of its children
// For convenience since in Unity dragging items into lists is bad
public class ToggleableGroup : Toggleable
{
    List<Toggleable> children;

    protected override void Start()
    {
        children = GetComponentsInChildren<Toggleable>().Skip(1).ToList();
    }

    public override void SetOn(bool state)
    {
        foreach (var child in children)
        {
            child.SetOn(state);
        }

        base.SetOn(state);
    }

    public override void TakeBool(bool value)
    {
        foreach (var child in children)
        {
            child.SetOn(value ^ child.invertReceivedBool);
        }

        // We XOR the received value and whether we invert it to get the new on value
        base.SetOn(value ^ invertReceivedBool);
    }
}
