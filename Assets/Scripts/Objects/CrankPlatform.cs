using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.U2D;

public class CrankPlatform : MonoBehaviour, IFloatAcceptor
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] Rigidbody2D crankHead;
    [SerializeField] CrankDirs forwardDir = CrankDirs.Clockwise; // Make clockwise considered "forward"
    [SerializeField] MoveTypes moveType = MoveTypes.ConstantSpeed;
    [SerializeField] [ShowIf("moveType", MoveTypes.ConstantSpeed)] [Tooltip("Units per crank")] float gearRatio;

    [ShowNonSerializedField] float along = 0;

    enum CrankDirs
    {
        Clockwise = -1,
        Counterclockwise = +1,
    }
    enum MoveTypes
    {
        ConstantSpeed,
        FractionComplete,
    }

    Rigidbody2D rb;

    Vector2 prevPos;
    Vector2 posDelta;
    float splineLength;
    float crankSign;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        splineLength = splineContainer.CalculateLength();
        crankSign = (int)forwardDir;
        prevPos = rb.position;
    }

    void FixedUpdate()
    {
        posDelta = rb.position - prevPos;
        prevPos = rb.position;
    }

    public void TakeFloat(float f)
    {
        // Move the platform according to the cranked distance
        if (moveType == MoveTypes.ConstantSpeed)
        {
            along += crankSign * f * gearRatio / splineLength;
        }
        else if (moveType == MoveTypes.FractionComplete)
        {
            along += crankSign * f;
        }
        along = Mathf.Clamp01(along);
        var pos = (Vector3)splineContainer.EvaluatePosition(along);
        rb.MovePosition(pos);
    }

    public void SetAlong(float newAlong)
    {
        along = newAlong;
        TakeFloat(0);
    }

    // Stop the pot from bouncing
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerMovement.instance.stuckTo == crankHead)
        {
            PlayerMovement.instance.flowerpot.MovePosition(PlayerMovement.instance.flowerpot.position + posDelta);
            PlayerMovement.instance.flowerpot.velocity = Vector2.zero;
        }
    }
}
