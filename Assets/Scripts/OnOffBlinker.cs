using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Lever;

public class OnOffBlinker : Toggleable
{
    enum BlinkMode
    {
        Symmetric,
        Asymmetric,
    }

    [Header("Blinker")]
    [SerializeField] float offsetTime = 0f;
    [SerializeField] BlinkMode blinkMode = BlinkMode.Asymmetric;
    [SerializeField] float onTime = 1f;
    [SerializeField][ShowIf("blinkMode", BlinkMode.Asymmetric)] float offTime = 1f;

    Timer timer;

    [SerializeField] GameObject[] subscriberObjects;
    List<IBoolAcceptor> subscribers;

    protected override void Start()
    {
        if (blinkMode == BlinkMode.Symmetric) offTime = onTime;

        subscribers = new List<IBoolAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IBoolAcceptor>());
        }

        if (offsetTime != 0)
            timer = Timer.Register(offsetTime, () => { base.Start(); });
        else
            base.Start();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);

        if (on)
        {
            OscillateOn();
        }
        else
        {
            if (timer != null) timer.Cancel();
        }
    }

    void OscillateOn()
    {
        UpdateSubscribers(true);
        timer = Timer.Register(onTime, () => { OscillateOff(); });
    }

    void OscillateOff()
    {
        UpdateSubscribers(false);
        timer = Timer.Register(offTime, () => { OscillateOn(); });
    }

    void UpdateSubscribers(bool state)
    {
        foreach (var sub in subscribers)
        {
            sub.TakeBool(state);
        }
    }
}
