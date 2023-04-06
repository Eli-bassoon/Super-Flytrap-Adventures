using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    [SerializeField] GameObject[] subscriberObjects;
    List<IBoolAcceptor> subscribers;

    public UnityEvent onTrigger;
    public UnityEvent onLeave;

    int containedEntities = 0;

    private void Start()
    {
        subscribers = new List<IBoolAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IBoolAcceptor>());
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (containedEntities == 0)
        {
            onTrigger.Invoke();
            foreach (var sub in subscribers)
            {
                sub.TakeBool(true);
            }
        }

        containedEntities++;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        containedEntities--;

        if (containedEntities == 0)
        {
            onLeave.Invoke();
            foreach (var sub in subscribers)
            {
                sub.TakeBool(false);
            }
        }
    }
}
