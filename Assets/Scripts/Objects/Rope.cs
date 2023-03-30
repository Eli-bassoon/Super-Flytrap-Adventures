using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public int numSegments = 1;
    public Rigidbody2D start;
    public Rigidbody2D end;

    [ReadOnly] public float length;

    [HideInInspector] public LineRenderer lineRenderer;

    [HideInInspector] public List<Transform> segmentTransforms;
    [HideInInspector] public List<DistanceJoint2D> segmentJoints;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        MakeRope();
    }

    void MakeRope()
    {
        lineRenderer.positionCount = numSegments + 1;

        // Transforms
        segmentTransforms = new List<Transform>(numSegments + 1) { start.transform };

        // Joints
        segmentJoints = new List<DistanceJoint2D>(numSegments);

        // Construct rope segments
        Vector2 startPos = start.position;
        length = (end.position - start.position).magnitude;
        Rigidbody2D segmentRb = start;
        for (int i = 0; i < numSegments - 1; i++)
        {
            GameObject go = new GameObject();
            go.transform.position = Vector2.Lerp(startPos, end.position, (i+1) / (float)numSegments);
            go.transform.parent = transform;
            var comp = go.AddComponent<DistanceJoint2D>();
            comp.distance = length / numSegments;

            comp.connectedBody = segmentRb;
            //comp.autoConfigureDistance = false;
            segmentRb = go.GetComponent<Rigidbody2D>();

            segmentJoints.Add(comp);
            segmentTransforms.Add(go.transform);
        }

        // Add final connection to this
        var comp2 = end.gameObject.AddComponent<DistanceJoint2D>();
        comp2.distance = length / numSegments;
        comp2.connectedBody = segmentRb;
        comp2.autoConfigureDistance = false;

        segmentJoints.Add(comp2);
        segmentTransforms.Add(end.transform);
    }

    void Update()
    {
        RenderRope();
    }

    void RenderRope()
    {
        Vector3[] points = segmentTransforms.Select(x => x.position).ToArray();
        lineRenderer.SetPositions(points);
    }

    public void UniformlyChangeLength(float newLength)
    {
        length = newLength;
        foreach (var segment in segmentJoints)
        {
            segment.distance = length / numSegments;
        }
    }
}
