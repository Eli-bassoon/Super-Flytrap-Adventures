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
            chase = true;
            maxWanderDistance = 100f;
            speed = 4;
            Vector3 towardsPlayer = (PlayerMovement.instance.transform.position - transform.position);
            rb.velocity = towardsPlayer.normalized * speed;
        }
        if (chase == true)
        {
            Vector3 towardsPlayer = (PlayerMovement.instance.transform.position - transform.position);
            rb.velocity = towardsPlayer.normalized * speed;
        }
        if (dist < 1)
        {
           Destroy(rb.gameObject);
        }
    }
}
