using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class PlayerNeckAnim : MonoBehaviour
{
    SplineContainer neckSpline;

    [SerializeField] [Range(0, 2f)] float headTangentStrength;
    [SerializeField] [Range(0, 1f)] float potTangentStrength;
    [SerializeField] Transform head;
    [SerializeField] Transform flowerpot;

    float3 zeroF3 = new float3(0, 0, 0);

    private void Awake()
    {
        neckSpline = GetComponent<SplineContainer>();
    }

    void Start()
    {

    }

    void Update()
    {
        // First knot at flowerpot
        Vector3 potPos = flowerpot.transform.TransformPoint(Vector2.up * 0.2f);
        Vector3 potTangent = flowerpot.transform.TransformVector(Vector2.up * potTangentStrength);
        BezierKnot potKnot = new BezierKnot(potPos, zeroF3, potTangent, Quaternion.identity);
        neckSpline.Spline.SetKnot(0, potKnot);

        // Second knot at head
        Vector3 headPos = head.transform.TransformPoint(Vector2.down * 0.25f);
        Vector3 headTangent = head.transform.TransformVector(Vector2.down * headTangentStrength);
        BezierKnot headKnot = new BezierKnot(headPos, headTangent, zeroF3, Quaternion.identity);
        neckSpline.Spline.SetKnot(1, headKnot);
    }
}
