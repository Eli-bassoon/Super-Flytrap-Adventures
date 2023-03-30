using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindPullerBlinds : BlindPuller
{
    [SerializeField] Transform mask;
    [SerializeField] Rope rope;

    float initialTop;
    float ropeX;

    protected override void Start()
    {
        startLengthOffset = mask.localScale.y;
        initialTop = mask.position.y + startLengthOffset / 2;

        if (rope != null)
        {
            ropeX = rope.start.position.x;
        }

        base.Start();
    }

    protected override void ChangeLength()
    {
        Vector3 newScale = mask.localScale;
        newScale.y = adjustedLength;
        mask.localScale = newScale;
        
        Vector2 newPos = mask.position;
        newPos.y = initialTop - adjustedLength / 2;
        mask.position = newPos;

        if (rope != null)
        {
            rope.start.MovePosition(new Vector2(ropeX, initialTop - adjustedLength));
        }
    }
}
