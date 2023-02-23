using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlindPuller : MonoBehaviour, IGrabHandler
{
    [SerializeField] float lengtheningSpeed;
    [SerializeField] float startLength = 0.1f;
    [SerializeField] float maxLength = 3;
    [SerializeField] int numSegments = 5;

    List<Transform> segmentTransforms;
    List<DistanceJoint2D> segmentJoints;
    Rigidbody2D rb;
    LineRenderer lineRenderer;
    bool grabbed;
    float prevLength;
    [ReadOnly] public float length;

    [SerializeField] GameObject[] subscriberObjects;
    List<IFloatAcceptor> subscribers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numSegments + 1;

        // Transforms
        segmentTransforms = new List<Transform>(numSegments+1) { transform.parent };

        // Joints
        segmentJoints = new List<DistanceJoint2D>(numSegments);
        length = startLength;
        prevLength = length;

        // Construct rope segments
        Vector2 position = transform.parent.position;
        Rigidbody2D segmentRb = transform.parent.GetComponent<Rigidbody2D>();
        for (int i = 0; i < numSegments-1; i++)
        {
            position.x += startLength / numSegments;
            GameObject go = new GameObject();
            go.transform.position = position;
            go.transform.parent = transform.parent;
            var comp = go.AddComponent<DistanceJoint2D>();
            comp.distance = length / numSegments;

            comp.connectedBody = segmentRb;
            segmentRb = go.GetComponent<Rigidbody2D>();

            segmentJoints.Add(comp);
            segmentTransforms.Add(go.transform);
        }

        // Add final connection to this
        position.x += startLength / numSegments;
        rb.position = position;
        var comp2 = gameObject.AddComponent<DistanceJoint2D>();
        comp2.distance = length / numSegments;
        comp2.connectedBody = segmentRb;

        segmentJoints.Add(comp2);
        segmentTransforms.Add(transform);

        // Process subscribers
        subscribers = new List<IFloatAcceptor>(subscriberObjects.Length);
        foreach (var sub in subscriberObjects)
        {
            subscribers.Add(sub.GetComponent<IFloatAcceptor>());
        }
    }

    void Update()
    {
        if (grabbed && (length < maxLength))
        {
            prevLength = length;
            length += Time.deltaTime * lengtheningSpeed;
            foreach (var segment in segmentJoints)
            {
                segment.distance = length / numSegments;
            }

            foreach (var sub in subscribers)
            {
                sub.TakeFloat((length - prevLength) / (maxLength - startLength));
            }
        }

        RenderRope();
    }

    void RenderRope()
    {
        Vector3[] points = segmentTransforms.Select(x => x.position).ToArray();
        lineRenderer.SetPositions(points);
    }

    public void OnGrab()
    {
        grabbed = true;
    }

    public void OnRelease()
    {
        grabbed = false;
    }
}
