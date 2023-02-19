using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class Bee : RandomWalkFlyingEnemy
{
    [SerializeField] private float trigger = 0;

    protected override void Start()
    {
        base.Start();

        speed = 5f;
        minMoveTime = 0.1f;
        maxMoveTime = 0.4f;
        maxWanderDistance = 1f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        float dist = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);

        if (dist < trigger)
        {
            maxWanderDistance = 100f;
            speed = 1;
            Vector3 towardsPlayer = (PlayerMovement.instance.transform.position - transform.position);
            rb.velocity = towardsPlayer.normalized * speed;
        }
    }
}
