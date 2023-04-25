using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scorpion : TimedCrawlingEnemy
{
    [SerializeField] UnityEvent onEatenEvent;

    public override void OnEaten()
    {
        onEatenEvent.Invoke();
    }
}
