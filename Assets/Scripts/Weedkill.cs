using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weedkill : MonoBehaviour, IFloatAcceptor
{
    Rigidbody2D rb;
    float rotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.SetRotation(rotation);
    }

    public void TakeFloat(float f) // float is the amount that the crank has cranked
    {
        rotation = -90 * f;
    }
}
