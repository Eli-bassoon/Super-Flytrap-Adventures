using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class CrankPlatform : MonoBehaviour
{
    [SerializeField] [Required] Crank crank;
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] [Tooltip("Units per crank")] float gearRatio;
    [SerializeField] CrankDirs forwardDir = CrankDirs.Clockwise; // Make clockwise considered "forward"

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

    void FixedUpdate()
    {
        // Move the platform according to the cranked distance
        float moveBy = crankSign * crank.turns * gearRatio / splineLength;
        float t = Mathf.Clamp01(moveBy);
        var pos = (Vector3)splineContainer.EvaluatePosition(t);
        rb.MovePosition(pos);
    }
}
