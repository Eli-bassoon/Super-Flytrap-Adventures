using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FloatingGrabbable : MonoBehaviour, IGrabHandler
{
    float reactivateDelay = 0.4f; // s

    CircleCollider2D thisCollider;

    void Start()
    {
        thisCollider = GetComponent<CircleCollider2D>();
    }

    public void OnGrab()
    {

    }

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
