using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlindPuller : MonoBehaviour, IGrabHandler
{
    [SerializeField] float lengtheningSpeed;
    [SerializeField] float maxLength = 3;
    [SerializeField] Rope rope;

    bool grabbed;
    float startLength;
    float prevLength;
    [ReadOnly] public float length;

    [SerializeField] GameObject[] subscriberObjects;
    List<IFloatAcceptor> subscribers;

    void Start()
    {
        // Process subscribers
        subscribers = new List<IFloatAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IFloatAcceptor>());
        }

        startLength = rope.length;
        print(startLength);
    }

    void Update()
    {
        if (grabbed && (length < maxLength))
        {
            prevLength = length;
            length += Time.deltaTime * lengtheningSpeed;
            rope.UniformlyLengthen(length);

            foreach (var sub in subscribers)
            {
                sub.TakeFloat((length - prevLength) / (maxLength - startLength));
            }
        }
    }

    public void OnGrab()
    {
        grabbed = true;
    }

    public void OnRelease()
    {
        grabbed = false;
    }
}
