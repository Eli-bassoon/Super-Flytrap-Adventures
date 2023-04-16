using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformOnStartup : MonoBehaviour
{
    [SerializeField] Vector2 position;
    [SerializeField] Vector2 velocity;
    [SerializeField] float rotation;
    [SerializeField] float angularVelocity;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.position = position;
        rb.velocity = velocity;
        rb.rotation = rotation;
        rb.angularVelocity = angularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
