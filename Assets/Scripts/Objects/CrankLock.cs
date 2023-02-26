using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankLock : MonoBehaviour, IGrabHandler
{
    FixedJoint2D lockingJoint;

    void Awake()
    {
        lockingJoint = GetComponent<FixedJoint2D>();
    }

    public void OnGrab()
    {
        lockingJoint.enabled = false;
    }

    public void OnRelease()
    {
        lockingJoint.enabled = true;
    }
}
