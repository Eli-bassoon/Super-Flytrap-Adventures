using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    enum LeverStates
    {
        Clockwise,
        Counterclockwise,
    }

    [SerializeField] float triggerAngle = 90f;
    [SerializeField] float deadZone = 10f;

    public Rigidbody2D handle;
    [SerializeField] GameObject[] subscriberObjects;
    List<IBoolAcceptor> subscribers;
    public UnityEvent LeverStateCWEvent;
    public UnityEvent LeverStateCCWEvent;

    Rigidbody2D rb;
    LeverStates prevState;
    [ShowNonSerializedField] float angle;
    [ShowNonSerializedField] LeverStates state;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        angle = GetAngle();
        state = GetState(angle);
        prevState = state;
        UpdateVisuals();

        subscribers = new List<IBoolAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IBoolAcceptor>());
        }
    }

    void FixedUpdate()
    {
        prevState = state;
        angle = GetAngle();
        state = GetState(angle);
        UpdateVisuals();

        if (prevState != state)
        {
            switch (state)
            {
                case LeverStates.Clockwise:
                    LeverStateCWEvent.Invoke();
                    break;

                case LeverStates.Counterclockwise:
                    LeverStateCCWEvent.Invoke();
                    break;
            }

            foreach (var sub in subscribers)
            {
                sub.TakeBool(state == LeverStates.Clockwise);
            }
        }
    }

    float GetAngle()
    {
        float givenAngle = Vector2.SignedAngle(transform.TransformDirection(Vector2.right), handle.position - rb.position);
        givenAngle = Utils.Angle0To360(givenAngle);
        return givenAngle;
    }

    LeverStates GetState(float rotation)
    {
        float modifiedAngle = Utils.Angle0To360(rotation - triggerAngle);

        // Not past dead zone
        if (modifiedAngle < deadZone)
        {
            return state;
        }

        // Gone past dead zone, can change state
        if ((modifiedAngle <= 180) && (modifiedAngle > 0))
        {
            return LeverStates.Counterclockwise;
        }
        else
        {
            return LeverStates.Clockwise;
        }
    }

    void UpdateVisuals()
    {
        handle.SetRotation(angle - 90);
    }
}
