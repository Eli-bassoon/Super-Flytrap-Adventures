using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A generic enemy that moves in a random walk flying pattern
public class EdibleBird : MonoBehaviour
{
    protected Vector3 startPos;
    protected Vector3 zeroVelocity = Vector3.zero;
    protected float currMoveTime;

    protected float speed;
    protected float minMoveTime;
    protected float maxMoveTime;
    protected float maxWanderDistance;

    private Rigidbody2D rb;
    private bool inMouth;

    protected void Start()
    {
        //base.Start();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        currMoveTime = 0;
        speed = 2f;
        minMoveTime = 0.1f;
        maxMoveTime = 0.4f;
        maxWanderDistance = 1f;

        inMouth = false;
    }

    protected void Update()
    {
        //base.Update();

    }

    protected void FixedUpdate()
    {
        //base.FixedUpdate();

        currMoveTime -= Time.deltaTime;
        // If we've moved for long enough, get a new direction and move towards it
        if (currMoveTime <= 0)
        {
            Vector3 towardsCenter = startPos - transform.position;
            float wanderDistance = towardsCenter.magnitude;

            Vector2 moveTowards;
            // Go towards the center if too far away
            if (inMouth)
            {
                moveTowards = Vector2.left;
                print("going left!");
            }
            else if (wanderDistance > maxWanderDistance)
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
        
        if (!inMouth && CharacterController.instance.mouthFull && CharacterController.instance.stuckTo == rb)
        {
            inMouth = true;
        }
        else if (inMouth && CharacterController.instance.stuckTo != rb)
        {
            inMouth = false;
        }

    }
}
   
