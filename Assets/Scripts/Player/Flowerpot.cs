using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowerpot : MonoBehaviour
{
    [Range(-.3f, 0)] [SerializeField] private float comOffset = -0.25f;

    public Rigidbody2D head;

    private Rigidbody2D rb;

    void Start()
    {
        // Change the center of mass to be lower
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector2(0, comOffset);
    }

    void FixedUpdate()
    {

    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
