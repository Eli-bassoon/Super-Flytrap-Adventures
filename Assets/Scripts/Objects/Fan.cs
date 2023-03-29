using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : Toggleable
{
    Animator anim;
    AreaEffector2D fanArea;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fanArea = GetComponent<AreaEffector2D>();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);

        fanArea.enabled = on;
        anim.SetBool("isOn", on);
    }
}
