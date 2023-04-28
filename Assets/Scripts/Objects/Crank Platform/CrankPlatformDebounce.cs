using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankPlatformDebounce : MonoBehaviour
{
    [SerializeField] Rigidbody2D crankHead;
    [HideInInspector] public bool playerGrabbing = false;

    bool movingPlayer = false;
    Vector2 potRelPos;

    void Start()
    {
        
    }

    public void OnGrab()
    {

    }

    public void OnRelease()
    {
        movingPlayer = false;
        PlayerMovement.instance.flowerpot.isKinematic = false;
    }

    // Forces the player to move with the platform
    private void FixedUpdate()
    {
        if (movingPlayer)
        {
            PlayerMovement.instance.flowerpot.position = (Vector2)transform.position + potRelPos;
            PlayerMovement.instance.flowerpot.rotation = 0;

            PlayerMovement.instance.flowerpot.angularVelocity = 0;
        }
    }

    // Stop the pot from bouncing
    void OnTriggerStay2D(Collider2D collision)
    {
        if (!movingPlayer &&
            collision.gameObject == PlayerMovement.instance.flowerpot.gameObject && 
            PlayerMovement.instance.stuckTo == crankHead)
        {
            movingPlayer = true;
            PlayerMovement.instance.flowerpot.isKinematic = true;
            potRelPos = PlayerMovement.instance.flowerpot.position - (Vector2)transform.position;
        }
    }
}
