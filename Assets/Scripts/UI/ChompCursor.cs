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

    Rigidbody2D playerRB;

    void Start()
    {
        playerRB = PlayerMovement.instance.flowerpot;
    }

    void Update()
    {
        // When mouth is full, disable cursor rendering
        if (PlayerMovement.instance.mouthFull)
        {
            cursor.enabled = false;
            clampedCursor.enabled = false;
        }

        // Otherwise, show if the player can grab where the mouse is
        else
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            cursor.enabled = true;
            cursor.transform.position = mousePosition;

            float dist = Vector2.Distance(playerRB.position, mousePosition);

            if (dist <= (PlayerMovement.instance.maxNeckLength + PlayerMovement.instance.maxTongueLength))
            {
                clampedCursor.enabled = false;
                cursor.sprite = validCursor;
            }
            else
            {
                cursor.sprite = invalidCursor;
            }
        }
    }
}
