using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Sara : MonoBehaviour
{
    [SerializeField] private float distUp = 0; //distance of platform
    [SerializeField] private float speed = 1.2f; //speed

    private float initialY;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() //better to do physics stuff in FixedUpdate
    {
        //every frame check below and above position
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
