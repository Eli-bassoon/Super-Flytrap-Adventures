using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

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
    float splineLength;
    float flySign;
    float splineTraveled = 0;
    float distThreshold = 0.1f;
    Vector2 splineStart;

    [SerializeField] SplineContainer splineContainer;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        currMoveTime = 0;
        speed = 2f;
        minMoveTime = 0.1f;
        maxMoveTime = 0.4f;
        maxWanderDistance = 1f;

        inMouth = false;

        splineLength = splineContainer.CalculateLength();
        flySign = 0;
        splineStart = (Vector3)splineContainer.EvaluatePosition(0);
    }

    protected void Update()
    {
        //base.Update();

    }

    protected void RandomMove()
    {
        // If we've moved for long enough, get a new direction and move towards it
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

    protected void PathMove()
    {
        //moveTowards = Vector2.left;
        //print("going left!");
        if (Vector2.Distance(transform.position, splineStart) > distThreshold && splineTraveled == 0)
        {
            Vector3 towardsCenter = splineStart - (Vector2)transform.position;
            Vector2 moveTowards = towardsCenter.normalized;
            rb.velocity = moveTowards * speed;
        }
        else
        {
            splineTraveled += (Time.fixedDeltaTime * speed / splineLength) * flySign;
            splineTraveled = Mathf.Clamp01(splineTraveled);
            Vector3 pos = (Vector3)splineContainer.EvaluatePosition(splineTraveled);
            rb.MovePosition(pos);
        }
    }

    protected void FixedUpdate()
    {
        //base.FixedUpdate();
        if (!inMouth && splineTraveled == 0) // we are not on the spline path
        {
            currMoveTime -= Time.fixedDeltaTime;
            // If we've moved for long enough, get a new direction and move towards it
            if (currMoveTime <= 0) { RandomMove(); }
        }
        else
        {
            if (inMouth) { flySign = 1;  }
            else { flySign = -1; }
            PathMove();
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
   
