using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlatformSimple : Toggleable
{
    Animator anim;
    Collider2D myCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);
        anim.SetBool("activated", state);
        myCollider.enabled = state;
    }
}
