using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public UnityEvent onTrigger;
    public UnityEvent onLeave;

    [SerializeField] bool flowerpotOnly = false;

    int containedParts = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (flowerpotOnly)
        {
            if (collision.gameObject.name == "Flowerpot")
                onTrigger.Invoke();
        }
        // Trigger when any part of the player enters for the first time
        else
        {
            if (containedParts == 0)
                onTrigger.Invoke();

            containedParts++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (flowerpotOnly)
        {
            if (collision.gameObject.name == "Flowerpot")
                onLeave.Invoke();
        }
        // Trigger when the last part of the player leaves
        else
        {
            containedParts--;

            if (containedParts == 0)
                onLeave.Invoke();
        }
    }
}
