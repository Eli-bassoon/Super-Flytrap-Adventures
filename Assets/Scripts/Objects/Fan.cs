using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    [OnValueChanged("SetState_")] public bool on = false;

    Animator anim;
    AreaEffector2D fanArea;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fanArea = GetComponent<AreaEffector2D>();
    }

    private void Start()
    {
        SetState(on);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOn()
    {
        SetState(true);
    }

    public void TurnOff()
    {
        SetState(false);
    }

    public void SetState(bool state)
    {
        on = state;

        fanArea.enabled = on;
        anim.SetBool("isOn", on);
    }

    void SetState_()
    {
        SetState(on);
    }
}
