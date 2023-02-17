using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGrabHandler
{
    public void OnGrab();

    public void OnRelease();
}
