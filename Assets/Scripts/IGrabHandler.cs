using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGrabHandler
{
    // Gets called whenever this object is grabbed by player
    public void OnGrab();

    // Gets called whenever this object is released by player
    public void OnRelease();
}
