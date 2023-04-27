using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class TriggerCamera : MonoBehaviour
{
    [SerializeField] public Vector2Int cameraBound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only save checkpoint when the flowerpot hits it
        if (collision.gameObject.CompareTag("Player") && collision.TryGetComponent(out Flowerpot _))
        {
            var cam = Camera.main.GetComponent<PixelPerfectCamera>();
            cam.refResolutionX = cameraBound.x;
            cam.refResolutionY = cameraBound.y;
        }
    }
}
