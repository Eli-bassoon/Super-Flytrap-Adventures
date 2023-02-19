using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_Clytie : MonoBehaviour
{
    [SerializeField] private float distRight = 0; // SerializeField gives you a little box to edit the value in the inspector
    [SerializeField] private float speed = 0.2f; // units per frame

    private float initialX;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        initialX = transform.position.x; // what Unity uses to capture the position & rotation of a body (Rigidbody2D and Unity are sometimes unsynced so always transform)
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() // because Unity reasons, physics objects work better with FixedUpdate than Update
    {
        if (transform.position.x > (initialX + distRight) )
        {
            speed *= -1;
        }
        else if (transform.position.x < initialX)
        {
            speed *= -1;
        }
        rb.velocity = new Vector2(speed, 0);
    }
}
