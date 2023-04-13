using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChompCursor : MonoBehaviour
{
    [SerializeField] SpriteRenderer cursor;
    [SerializeField] SpriteRenderer clampedCursor;
    [Space]
    [SerializeField] Sprite validCursor;
    [SerializeField] Sprite invalidCursor;
    [SerializeField] Sprite neutralCursor;
    [SerializeField][Range(0, 0.5f)] float linecastCushion = 0.1f;

    Rigidbody2D playerRB;
    Rigidbody2D flowerpot;
    float maxLength;

    void Start()
    {
        playerRB = PlayerMovement.instance.GetComponent<Rigidbody2D>();
        flowerpot = PlayerMovement.instance.flowerpot;
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        cursor.transform.position = mousePosition;

        // When mouth is full, disable cursor rendering
        if (PlayerMovement.instance.mouthFull || PlayerMovement.instance.tongueOut)
        {
            cursor.enabled = true;
            clampedCursor.enabled = false;
            cursor.sprite = neutralCursor;
        }

        // Otherwise, show if the player can grab where the mouse is
        else
        {
            maxLength = (PlayerMovement.instance.neckDistanceJointLength + PlayerMovement.instance.maxTongueLength) * 1.05f;

            // Start offset from the line if the mouse is far enough away
            Vector2 linecastStart = playerRB.position;
            if (Vector2.Distance(playerRB.position, mousePosition) > linecastCushion)
            {
                Vector2 mouseDirection = (mousePosition - playerRB.position).normalized;
                linecastStart += mouseDirection * linecastCushion;
            }

            // See if we would hit
            RaycastHit2D closestHit = Physics2D.Linecast(linecastStart, mousePosition, PlayerMovement.instance.grabbableLayers);

            // Get the closest point we hit
            Vector2 closestHitPoint;
            if (closestHit.collider != null)
            {
                if (!PlayerMovement.instance.stuck || (Vector2.Distance(closestHit.point, playerRB.position) > linecastCushion*1.05f))
                {
                    closestHitPoint = closestHit.point;
                }
                else
                {
                    closestHitPoint = playerRB.position;
                }
            }
            else
            {
                closestHitPoint = mousePosition;
            }

            float potDist = Vector2.Distance(flowerpot.position, closestHitPoint);

            // The hit position is in range
            if (potDist <= maxLength)
            {
                clampedCursor.enabled = true;
                clampedCursor.transform.position = closestHitPoint;
                clampedCursor.sprite = validCursor;

                if (closestHit.collider != null)
                {
                    cursor.enabled = true;
                    cursor.sprite = invalidCursor;

                    if (!PlayerMovement.instance.CanGrabOnto(closestHit))
                    {
                        clampedCursor.sprite = invalidCursor;
                    }
                }
                else
                {
                    cursor.enabled = false;
                }
            }
            // The hit position is out of range
            else
            {
                clampedCursor.enabled = true;
                cursor.enabled = true;
                cursor.sprite = invalidCursor;

                // Get clamped position using math magic
                float xi = playerRB.position.x;
                float yi = playerRB.position.y;
                float xf = mousePosition.x;
                float yf = mousePosition.y;
                float xc = flowerpot.position.x;
                float yc = flowerpot.position.y;
                float L = maxLength;

                float dx = xf - xi;
                float dy = yf - yi;

                float t = ((xc - xi) * dx + (yc - yi) * dy + Mathf.Sqrt(L*L * (dx*dx + dy*dy) -
                    Mathf.Pow(-xi * yc - xc * yf + xi * yf + xf * (yc - yi) + xc * yi, 2)))
                    / (dx*dx + dy*dy);

                Vector2 toClamped = Vector2.LerpUnclamped(playerRB.position, mousePosition, t);
                clampedCursor.transform.position = toClamped;
            }
        }
    }
}
