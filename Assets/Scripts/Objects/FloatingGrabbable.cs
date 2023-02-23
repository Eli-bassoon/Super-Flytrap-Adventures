using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FloatingGrabbable : MonoBehaviour, IGrabHandler
{
    float reactivateDelay = 0.4f; // s

    Collider2D thisCollider;

    void Awake()
    {
        thisCollider = GetComponent<Collider2D>();
    }

    public void OnGrab()
    {

    }

    // On release, deactivates the collider for a short time period so the player doesn't double grab it
    public void OnRelease()
    {
        Deactivate();
        Timer.Register(reactivateDelay, () => Activate());
    }

    void Activate()
    {
        thisCollider.enabled = true;
    }

    void Deactivate()
    {
        thisCollider.enabled = false;
    }
}
