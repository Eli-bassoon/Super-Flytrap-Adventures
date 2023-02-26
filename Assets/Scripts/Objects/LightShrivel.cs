using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShrivel : LightActivated
{
    Animator anim;
    Collider2D myCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    protected override void OnShineStart()
    {
        anim.SetBool("activated", true);
        myCollider.enabled = true;
    }

    protected override void OnShineStay() { }

    protected override void OnShineEnd()
    {
        anim.SetBool("activated", false);
        myCollider.enabled = false;
    }
}
