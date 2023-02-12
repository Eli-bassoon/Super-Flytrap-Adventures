using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Nathan : MonoBehaviour
{

    [SerializeField] private float distUp = 0;
    [SerializeField] private float speed = 0.2f;

    private float initialY;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y > (initialY + distUp))
        {
            speed = -speed;
        }
        else if (transform.position.y < initialY)
        {
            speed = -speed;
        }

        rb.velocity = new Vector2(0, speed);
    }
}
