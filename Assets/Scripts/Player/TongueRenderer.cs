using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueRenderer : MonoBehaviour
{
    [SerializeField] Transform tongueOrigin;

    LineRenderer lineRenderer;
    Vector3[] positions = new Vector3[2];

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Rendering tongue
        if (PlayerMovement.instance.tongueOut)
        {
            lineRenderer.enabled = true;
            RenderTongue();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
    
    void RenderTongue()
    {
        positions[0] = tongueOrigin.position;
        positions[1] = transform.position;

        lineRenderer.SetPositions(positions);
    }
}
