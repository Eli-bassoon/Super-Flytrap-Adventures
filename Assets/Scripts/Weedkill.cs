using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weedkill : MonoBehaviour, IFloatAcceptor
{
    Rigidbody2D rb;
    float rotation = -90f;
    [SerializeField] float gearRatio = -45;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.SetRotation(rotation);
    }

    // Update is called once per frame
    void Update()
    {
        rb.SetRotation(rotation);
    }

    public void TakeFloat(float f) // float is the amount that the crank has cranked
    {
        rotation += gearRatio * f;
        rotation = Mathf.Clamp(rotation, -90, 0);
    }
}
