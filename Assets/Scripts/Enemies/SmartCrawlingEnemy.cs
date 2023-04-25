using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCrawlingEnemy : Enemy
{
    public Transform check;
    private LayerMask groundMask;
    protected float speed;
    public int direction = 1;
    protected float ledgeWaitTime = 1f;
    protected bool waitingAtBoundary = false;

    protected override void Start()
    {
        base.Start();

        groundMask = LayerMask.GetMask("Ground", "Default");

        // Flips the enemy to the initial direction
        Vector3 theScale = transform.localScale;
        theScale.x *= direction;
        transform.localScale = theScale;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!waitingAtBoundary)
        {
            // If we're at a ledge or a wall, we're considered at a boundary
            waitingAtBoundary = CheckAtLedge() || CheckAtWall();

            // We're not waiting, so update the velocity like normal
            if (!waitingAtBoundary)
            {
                rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            }
            else
            {
                StartCoroutine(WaitAtBoundary());
            }
        }
    }

    protected void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Are we at a ledge
    protected bool CheckAtLedge()
    {
        RaycastHit2D hit = Physics2D.Raycast(check.position, -Vector2.up, 1f, groundMask);
        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    // Did we hit a wall
    protected bool CheckAtWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(check.position, direction * Vector2.right, 0.001f, groundMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    IEnumerator WaitAtBoundary()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(ledgeWaitTime);
        direction = -direction;
        Flip();
        waitingAtBoundary = false;
    }
}
