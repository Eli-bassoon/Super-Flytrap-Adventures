using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabHandler : MonoBehaviour, IGrabHandler
{
    public UnityEvent onGrab;
    public UnityEvent onRelease;

    public void OnGrab()
    {
        onGrab.Invoke();
    }

    public void OnRelease()
    {
        onRelease.Invoke();
    }
}
