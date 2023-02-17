using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowerpot : MonoBehaviour
{
    [Range(-.3f, 0)][SerializeField] private float comOffset = -0.25f;
    [SerializeField] private float upwardsForceThreshold = 1f;
    [SerializeField] private float upwardsForce = 20f;
    [SerializeField] private float swingForce = 10f;

    public Rigidbody2D head;

    private Rigidbody2D rb;
    private CharacterController controller;

    private float distanceToHead;

    private Vector2 clickPosition;

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

            clickPosition = Input.mousePosition;
        }
        else
        {

            //apply impulse whenever touching away from head
            // Vector2 currentMousePosition = Input.mousePosition;

            // float angle = AngleBetweenVector2(clickPosition, currentMousePosition);

            // float dist = Vector2.Distance(currentMousePosition, clickPosition);
            // dist = Mathf.Min(swingForce, dist);
            // if (dist < 2)
            // {
            //     dist = 0;
            // }

            // Vector2 impulse = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;

            // rb.AddForce(impulse, ForceMode2D.Impulse);


        }


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, upwardsForceThreshold);
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }


}



