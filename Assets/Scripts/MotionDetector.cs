using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionDetector : MonoBehaviour
{
    public UnityEvent onTrigger;
    public UnityEvent onLeave;

    void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger.Invoke();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        onLeave.Invoke();
    }
}
