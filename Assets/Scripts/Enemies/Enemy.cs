using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for an enemy
public abstract class Enemy : MonoBehaviour
{
    public bool edible = true;
    public int damage = 10;
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

    public virtual void OnEaten()
    {

    }
}
