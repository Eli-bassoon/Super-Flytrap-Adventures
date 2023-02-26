using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crank : MonoBehaviour
{
    [SerializeField] Transform crankBase;
    [SerializeField] Transform crankHead;

    // In degrees
    [ReadOnly] public float angle;
    [ReadOnly] public float clampedAngle;
    [ReadOnly] public float absAngle;
    [ReadOnly] public float turns;
    [ReadOnly] public int wholeTurns;

    [SerializeField] GameObject[] turnSubscriberObjects;

    List<IFloatAcceptor> turnSubscribers;

    private float startAngle;
    private float prevClampedAngle;
    private float prevTurns;

    void Start()
    {
        startAngle = GetAbsAngle();

        turnSubscribers = new List<IFloatAcceptor>(turnSubscriberObjects.Length);
        foreach (var sub in turnSubscriberObjects)
        {
            turnSubscribers.Add(sub.GetComponent<IFloatAcceptor>());
        }
    }

    void FixedUpdate()
    {
        prevClampedAngle = clampedAngle;
        prevTurns = turns;

        clampedAngle = GetRelAngle();
        absAngle = GetAbsAngle();

        float angleDelta = clampedAngle - prevClampedAngle;
        // Stop clipping
        if (Mathf.Abs(angleDelta) > 180)
        {
            if (angleDelta > 0)
            {
                wholeTurns--;
            }
            else
            {
                wholeTurns++;
            }
        }

        angle = wholeTurns * 360 + clampedAngle;
        turns = angle / 360;
        if (prevTurns != turns)
        {
            foreach (var sub in turnSubscribers)
            {
                sub.TakeFloat(turns - prevTurns);
            }
        }
    }

    float GetAbsAngle()
    {
        Vector2 pointingTowards = crankHead.position - crankBase.position;
        float tempAngle = Mathf.Atan2(pointingTowards.y, pointingTowards.x) * Mathf.Rad2Deg;
        tempAngle = (tempAngle + 360) % 360;
        return tempAngle;
    }

    float GetRelAngle()
    {
        float tempAngle = GetAbsAngle();
        tempAngle -= startAngle;
        tempAngle = (tempAngle + 360) % 360;
        return tempAngle;
    }
}
