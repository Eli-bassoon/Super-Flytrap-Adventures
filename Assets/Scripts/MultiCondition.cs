using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiCondition : Toggleable
{
    [SerializeField] int numConditions = 2;
    int conditionsFilled = 0;

    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    public void FillCondition()
    {
        conditionsFilled++;
        CheckFilled();
    }

    public void UnfillCondition()
    {
        conditionsFilled--;
        CheckFilled();
    }

    public override void TakeBool(bool value)
    {
        if (value)
        {
            conditionsFilled++;
        }
        else
        {
            conditionsFilled--;
        }
        CheckFilled();
    }

    // When something changes, check if everything is filled
    void CheckFilled()
    {
        if (conditionsFilled >= numConditions && !on)
        {
            TurnOn();
        }
        else if (on)
        {
            TurnOff();
        }
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);

        if (on)
        {
            onActivate.Invoke();
        }
        else
        {
            onDeactivate.Invoke();
        }
    }
}
