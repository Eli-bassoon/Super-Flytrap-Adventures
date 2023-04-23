using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowToggle : Toggleable
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField][Label("Moving Rigidbody")] Rigidbody2D rb;
    [SerializeField] float moveTime = 1f;

    float along = 0;
    Coroutine cr;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    public override void SetOn(bool state)
    {
        base.SetOn(state);
        if (cr != null) StopCoroutine(cr);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while ((on && along < 1) || (!on && along > 0))
        {
            // Change along position
            float changeBy = Time.fixedDeltaTime / moveTime;

            if (on) along += changeBy;
            else along -= changeBy;
            along = Mathf.Clamp01(along);

            // Move to new position
            var pos = (Vector3)splineContainer.EvaluatePosition(along);
            rb.MovePosition(pos);

            yield return new WaitForFixedUpdate();
        }
    }
}
