using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

// A generic enemy that moves in a random walk flying pattern
public abstract class RandomWalkFlyingEnemy : Enemy
{
    protected Vector3 startPos;
    protected Vector3 zeroVelocity = Vector3.zero;
    protected float currMoveTime;

    protected float speed;
    protected float minMoveTime;
    protected float maxMoveTime;
    protected float maxWanderDistance;

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        currMoveTime = 0;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        currMoveTime -= Time.deltaTime;
        // If we've moved for long enough, get a new direction and move towards it
        if (currMoveTime <= 0)
        {
            Vector3 towardsCenter = startPos - transform.position;
            float wanderDistance = towardsCenter.magnitude;

            Vector2 moveTowards;
            // Go towards the center if too far away
            if (wanderDistance > maxWanderDistance)
            {
                moveTowards = towardsCenter.normalized;
            }
            // Move in a random direction
            else
            {
                moveTowards = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            }
            rb.velocity = moveTowards * speed;
            currMoveTime = Random.Range(minMoveTime, maxMoveTime);
        }
    }
}
