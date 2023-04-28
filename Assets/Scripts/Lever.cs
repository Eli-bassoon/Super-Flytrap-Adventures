using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Lever : MonoBehaviour
{
    public enum LeverStates
    {
        Clockwise = +1,
        Counterclockwise = -1,
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
    float length;
    [ShowNonSerializedField] float angle;
    [OnValueChanged("SetState_")] public LeverStates state = LeverStates.Clockwise;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        angle = GetAngle();
        state = GetState(angle);
        prevState = state;
        length = Vector2.Distance(rb.position, handle.position);
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

    public void SetAngle(float newAngle)
    {
        angle = newAngle;
        Vector3 newHandleRelPos = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle) * Vector2.right * length;
        handle.transform.localPosition = newHandleRelPos;
        handle.transform.localEulerAngles = new Vector3(0, 0, transform.eulerAngles.z + angle - 90);
    }

    public void SetState(LeverStates newState)
    {
        float angleOffset = deadZone * 1.5f;

        switch (newState)
        {
            case LeverStates.Clockwise:
                SetAngle(triggerAngle - angleOffset);
                break;

            case LeverStates.Counterclockwise:
                SetAngle(triggerAngle + angleOffset);
                break;
        }
    }

    public void SetState(int newState)
    {
        SetState((LeverStates)newState);
    }

    void SetState_()
    {
        rb = GetComponent<Rigidbody2D>();
        length = Vector2.Distance(transform.position, handle.transform.position);
        SetState(state);
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
        handle.SetRotation(rb.rotation + angle - 90);
    }
}
