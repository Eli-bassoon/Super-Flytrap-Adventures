using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankPlatform : MonoBehaviour
{
    [SerializeField] [Required] Crank crank;
    [SerializeField] [Tooltip("The direction that a clockwise turn will go")] Vector2 moveDir;
    [SerializeField] [Tooltip("Units per crank")] float gearRatio;

    Rigidbody2D rb;

    Vector2 startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveDir = moveDir.normalized;
        startPos = rb.position;
    }

    void FixedUpdate()
    {
        float moveBy = -crank.turns * gearRatio; // Make clockwise considered "forward"
        rb.MovePosition(startPos + moveDir * moveBy);
    }
}
