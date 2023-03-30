using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindPullerRope : BlindPuller
{
    [SerializeField] Rope rope;

    protected override void Start()
    {
        startLengthOffset = rope.length;

        base.Start();
    }

    protected override void ChangeLength()
    {
        GetComponent<Rigidbody2D>().WakeUp();
        rope.UniformlyChangeLength(adjustedLength);
    }
}
