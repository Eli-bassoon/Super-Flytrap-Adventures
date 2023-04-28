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
        IEnumerator Zoom()
        {
            var cam = Camera.main.GetComponent<PixelPerfectCamera>();
            while (cam.refResolutionX != cameraBound.x || cam.refResolutionY != cameraBound.y)
            {
                if (cameraBound.x > cam.refResolutionX) 
                {
                    cam.refResolutionX = (int)Mathf.Lerp(cam.refResolutionX, cam.refResolutionX + 1, 1);
                    cam.refResolutionY = (int)Mathf.Lerp(cam.refResolutionY, cam.refResolutionY + 1, 1);
                }
                else
                {
                    cam.refResolutionX = (int)Mathf.Lerp(cam.refResolutionX, cam.refResolutionX - 1, 1);
                    cam.refResolutionY = (int)Mathf.Lerp(cam.refResolutionY, cam.refResolutionY - 1, 1);
                }
                yield return null;
            }
        }

        // Only save checkpoint when the flowerpot hits it
        if (collision.gameObject.CompareTag("Player") && collision.TryGetComponent(out Flowerpot _))
        {
            StartCoroutine(Zoom());
        }
    }
}
