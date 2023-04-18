using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weedkill : MonoBehaviour, IFloatAcceptor
{
    Rigidbody2D rb;
    float rotation;
    [SerializeField] float gearRatio = -45;
    [SerializeField][Range(0, 90)] float disableSludgeAngle = 45f;
    [SerializeField] Sludgefall sludgefall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotation = rb.rotation;
    }

    void Update()
    {
        
    }

    public void TakeFloat(float f) // float is the amount that the crank has cranked
    {
        rotation += gearRatio * f;
        rotation = Mathf.Clamp(rotation, -90, 0);

        if (rotation < -disableSludgeAngle)
        {
            sludgefall.TurnOff();
        }
        else
        {
            sludgefall.TurnOn();
        }

        rb.SetRotation(rotation);
    }
}
