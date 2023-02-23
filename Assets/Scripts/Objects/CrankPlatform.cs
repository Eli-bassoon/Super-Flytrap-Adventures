using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class CrankPlatform : MonoBehaviour, IFloatAcceptor
{
    //[SerializeField] [Required] Crank crank;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] [Tooltip("Units per crank")] float gearRatio;
    [SerializeField] CrankDirs forwardDir = CrankDirs.Clockwise; // Make clockwise considered "forward"

    float along = 0;

    enum CrankDirs
    {
        Clockwise = -1,
        Counterclockwise = +1,
    }

    Rigidbody2D rb;

    float splineLength;
    float crankSign;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        splineLength = splineContainer.CalculateLength();
        crankSign = (int)forwardDir;
    }

    public void TakeFloat(float turnsChange)
    {
        // Move the platform according to the cranked distance
        along += crankSign * turnsChange * gearRatio / splineLength;
        along = Mathf.Clamp01(along);
        var pos = (Vector3)splineContainer.EvaluatePosition(along);
        rb.MovePosition(pos);
    }
}
