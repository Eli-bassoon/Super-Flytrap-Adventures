using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionDetector : MonoBehaviour
{
    public UnityEvent onTrigger;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.GetMask("Player"))
        {
            onTrigger.Invoke();
        }
    }
}
