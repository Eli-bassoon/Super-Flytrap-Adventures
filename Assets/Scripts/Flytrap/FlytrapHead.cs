using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlytrapHead : MonoBehaviour
{

    [SerializeField] public float pullStrength = 8f;
    [SerializeField] public float pullRadius = 1f;

    public Rigidbody2D rb;

    public Vector2 mousePos;

    public Vector2 pullForce;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            mousePos = Vector2.zero;
        }
        // pullForce = (mousePos - rb.position);
        // if (Input.GetMouseButton(0))
        // {
        //     if (pullForce.magnitude > pullRadius)
        //     {
        //         pullForce = pullForce.normalized * pullRadius;
        //     }
        // }
        // else
        // {
        //     pullForce *= 0;
        // }

        // pullForce *= pullStrength;
        // Debug.Log(pullForce.ToString());

    }

    private void FixedUpdate()
    {
        // rb.AddForce(pullForce, ForceMode2D.Impulse);
        if (Input.GetMouseButton(0))
        {
            rb.MovePosition(mousePos);
        }

    }

}