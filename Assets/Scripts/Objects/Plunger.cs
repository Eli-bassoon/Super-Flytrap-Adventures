using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Plunger : MonoBehaviour
{
    [SerializeField] Rigidbody2D handle;
    [SerializeField] float depressedThreshold = 0.9f;

    [SerializeField] GameObject[] subscriberObjects;
    List<IBoolAcceptor> subscribers;
    public UnityEvent PushedInEvent;
    public UnityEvent PulledOutEvent;

    Rigidbody2D rb;
    float activateDist;

    [ReadOnly] public bool activated = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        activateDist = depressedThreshold * GetComponent<SliderJoint2D>().limits.max;

        subscribers = new List<IBoolAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IBoolAcceptor>());
        }
    }

    void FixedUpdate()
    {
        if (Vector2.Distance(rb.position, handle.position) >= activateDist)
        {
            if (!activated)
            {
                activated = true;
                PushedInEvent.Invoke();
                foreach (var sub in subscribers)
                {
                    sub.TakeBool(activated);
                }
            }
        }
        else
        {
            if (activated)
            {
                activated = false;
                PulledOutEvent.Invoke();
                foreach (var sub in subscribers)
                {
                    sub.TakeBool(activated);
                }
            }
        }
    }
}
