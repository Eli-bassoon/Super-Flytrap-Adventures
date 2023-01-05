using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowerpot : MonoBehaviour
{
    [Range(-.3f, 0)] [SerializeField] private float comOffset = -0.25f;
    [SerializeField] private float upwardsForceThreshold = 1f;
    [SerializeField] private float upwardsForce = 10f;

    public Rigidbody2D head;

    private Rigidbody2D rb;
    private CharacterController controller;

    private float distanceToHead;

    void Start()
    {
        // Change the center of mass to be lower
        rb = GetComponent<Rigidbody2D>();
        controller = head.GetComponent<CharacterController>();
        rb.centerOfMass = new Vector2(0, comOffset);
    }

    void FixedUpdate()
    {
        // Apply a force and change the center of mass of the head if it's close enough
        distanceToHead = Vector3.Distance(transform.position, head.position);

        if (!controller.stuck)
        {
            // Pushes upwards if the head is near the pot
            if (distanceToHead < upwardsForceThreshold)
            {
                head.AddForce(new Vector2(0, upwardsForce));
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, upwardsForceThreshold);
    }
}
