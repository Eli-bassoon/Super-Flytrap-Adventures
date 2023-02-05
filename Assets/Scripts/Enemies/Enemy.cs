using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for an enemy
public abstract class Enemy : MonoBehaviour
{
    public bool edible = true;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }
}
