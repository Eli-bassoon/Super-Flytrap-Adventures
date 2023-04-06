using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDetector : MonoBehaviour
{
    [SerializeField] protected Transform[] lights;
    [SerializeField] protected LayerMask blockingLayers;

    protected float[] maxAngles;
    protected float extraSpace = 5f;
    [ReadOnly] public bool lightShining = false;
    protected bool prevLightShining;

    protected void Start()
    {
        maxAngles = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
            maxAngles[i] = lights[i].GetComponentInChildren<Light2D>().pointLightOuterAngle / 2 + extraSpace;

        // Initialize lights shining
        lightShining = TestForLight();
        prevLightShining = lightShining;

        if (lightShining)
            OnShineStart();
        else
            OnShineEnd();
    }

    protected void FixedUpdate()
    {
        prevLightShining = lightShining;
        lightShining = TestForLight();

        // Currently shining
        if (lightShining)
        {
            // Was previously shining
            if (prevLightShining)
                OnShineStay();
            // Wasn't previously shining
            else
                OnShineStart();
        }
        // Not currently shining
        else if (prevLightShining)
            OnShineEnd();
    }

    protected bool TestForLight()
    {
        Vector2 toPosition;
        float angleToLight;
        for (int i = 0; i < lights.Length; i++)
        {
            // This math is bad. Change it.
            toPosition = transform.position - lights[i].position;
            angleToLight = Vector2.Angle(toPosition, -lights[i].transform.up);

            // Tests if a particular light is shining on it
            if ((lights[i].GetComponentInChildren<Light2D>().enabled) && // The lamp is on
                (angleToLight <= maxAngles[i]) && // In angle range
                !Physics2D.Linecast(transform.position, lights[i].position, blockingLayers)) // Nothing blocking
            {
                return true;
            }
        }

        return false;
    }

    protected virtual void OnShineStart() { }

    protected virtual void OnShineStay() { }

    protected virtual void OnShineEnd() { }
}
