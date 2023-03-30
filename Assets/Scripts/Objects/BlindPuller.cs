using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BlindPuller : MonoBehaviour, IGrabHandler, IFloatAcceptor
{
    [SerializeField] protected float lengtheningSpeed;
    [SerializeField] protected float maxLength = 3;
    [SerializeField] protected bool startAtBottom = false;

    protected bool grabbed;
    [ShowNonSerializedField] protected float startLengthOffset;
    protected float prevLength;
    [ReadOnly] public float length = 0;
    protected float adjustedLength
    {
        get { return length + startLengthOffset; }
    }

    [SerializeField] protected GameObject[] subscriberObjects;
    protected List<IFloatAcceptor> subscribers;

    protected virtual void Start()
    {
        // Process subscribers
        subscribers = new List<IFloatAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IFloatAcceptor>());
        }

        if (startAtBottom) {
            length = maxLength;
            startLengthOffset -= maxLength;
        }
    }

    void Update()
    {
        if (grabbed && (length < maxLength))
        {
            prevLength = length;
            length += Time.deltaTime * lengtheningSpeed;
            ChangeLength();

            foreach (var sub in subscribers)
            {
                sub.TakeFloat((length - prevLength) / maxLength);
            }
        }
    }

    protected abstract void ChangeLength();

    public void OnGrab()
    {
        grabbed = true;
    }

    public void OnRelease()
    {
        grabbed = false;
    }

    public void TakeFloat(float f)
    {
        length -= maxLength * f;
        ChangeLength();
    }
}
