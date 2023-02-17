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

    private float startAngle;
    private float prevClampedAngle;

    void Start()
    {
        startAngle = GetAbsAngle();
    }

    void FixedUpdate()
    {
        prevClampedAngle = clampedAngle;
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
