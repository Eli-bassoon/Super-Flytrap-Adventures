using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankPlatformDebounce : MonoBehaviour
{
    [SerializeField] Rigidbody2D crankHead;

    Vector2? potRelPos;

    void Start()
    {
        
    }

    // Stop the pot from bouncing
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerMovement.instance.flowerpot.gameObject && PlayerMovement.instance.stuckTo == crankHead)
        {
            if (potRelPos == null)
            {
                potRelPos = PlayerMovement.instance.flowerpot.position - (Vector2)transform.position;
            }

            PlayerMovement.instance.flowerpot.MovePosition((Vector2)transform.position + (Vector2)potRelPos);
            PlayerMovement.instance.flowerpot.velocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerMovement.instance.flowerpot.gameObject)
        {
            potRelPos = null;
        }
    }
}
