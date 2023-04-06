using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    const int CIRC_DEG = 360;

    public static float Angle0To360(float angle)
    {
        if (angle < 0)
        {
            angle += CIRC_DEG;
        }

        return angle;
    }
}
