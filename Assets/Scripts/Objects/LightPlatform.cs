using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlatform : LightActivated
{
    Animator anim;
    Collider2D myCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    protected override void OnActivate()
    {
        anim.SetBool("activated", true);
        myCollider.enabled = true;
    }

    protected override void OnDeactivate()
    {
        anim.SetBool("activated", false);
        myCollider.enabled = false;
    }
}

