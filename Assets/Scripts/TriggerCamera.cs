using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class TriggerCamera : MonoBehaviour
{
    [SerializeField] Vector2Int cameraBound;
    [SerializeField] float zoomTime = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only save checkpoint when the flowerpot hits it
        if (collision.gameObject.CompareTag("Player") && collision.TryGetComponent(out Flowerpot _))
        {
            StartCoroutine(Zoom());
        }
    }

    IEnumerator Zoom()
    {
        var cam = Camera.main.GetComponent<PixelPerfectCamera>();

        Vector2Int startRes = new Vector2Int(cam.refResolutionX, cam.refResolutionY);

        float t;
        float startTime = Time.time;

        while ((t = (Time.time - startTime) / zoomTime) < zoomTime)
        {
            cam.refResolutionX = (int)Mathf.Lerp(startRes.x, cameraBound.x, t);
            cam.refResolutionY = (int)Mathf.Lerp(startRes.y, cameraBound.y, t);

            yield return new WaitForEndOfFrame();
        }
    }
}
