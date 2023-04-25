using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCrawlingEnemy : Enemy
{
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float moveTime = 5f;
    public int direction = 1;

    protected float ledgeWaitTime = 1f;
    protected float startMoveTime = 0;
    protected bool waiting = false;

    protected override void Start()
    {
        base.Start();

        // Flips the enemy to the initial direction
        Vector3 theScale = transform.localScale;
        theScale.x *= direction;
        transform.localScale = theScale;

        startMoveTime = Time.time;
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!waiting)
        {
            if (Time.time - startMoveTime >= moveTime)
            {
                StartCoroutine(WaitAtBoundary());
            }
            else
            {
                rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            }
        }
    }

    protected void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator WaitAtBoundary()
    {
        waiting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(ledgeWaitTime);
        direction = -direction;
        Flip();
        startMoveTime = Time.time;
        waiting = false;
    }
}
