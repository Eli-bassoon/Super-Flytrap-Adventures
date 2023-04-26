using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankPlatformHandle : MonoBehaviour, IGrabHandler
{
    [SerializeField] CrankPlatformDebounce debouncer;

    public void OnGrab()
    {
        debouncer.OnGrab();
    }

    public void OnRelease()
    {
        debouncer.OnRelease();
    }
}
