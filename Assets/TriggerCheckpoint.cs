using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCheckpoint : MonoBehaviour
{
    public UnityEvent onRespawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DamageHandler.instance.SaveCheckpoint(transform);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Checkpoint.png", true);
        Bounds bounds = GetComponent<CompositeCollider2D>().bounds;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
