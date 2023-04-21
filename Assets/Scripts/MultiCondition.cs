using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiCondition : Toggleable
{
    [SerializeField] int numConditions = 2;
    public int conditionsFilled = 0;
    public int filledValue = 0;

    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    void Awake()
    {
        for (int i = 0; i < numConditions; i++)
        {
            filledValue |= 1 << i;
        }
    }

    public void FillCondition(int condNum)
    {
        conditionsFilled |= 1 << condNum;
        CheckFilled();
    }

    public void UnfillCondition(int condNum)
    {
        conditionsFilled ^= 1 << condNum;
        CheckFilled();
    }

    public override void TakeBool(bool value)
    {
        throw new System.Exception("Do not use TakeBool with MultiCondition.cs!");
    }

    // When something changes, check if everything is filled
    void CheckFilled()
    {
        if (conditionsFilled == filledValue && !on)
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
