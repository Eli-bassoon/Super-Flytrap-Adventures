using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheel : Toggleable
{
    [SerializeField] private float rotationalSpeed = 2f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (on)
            rb.MoveRotation(rb.rotation + rotationalSpeed * Time.fixedDeltaTime);
    }
}
