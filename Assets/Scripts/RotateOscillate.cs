using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOscillate : MonoBehaviour
{
    [SerializeField] float rotationalSpeed = 2f;
    [SerializeField][MinMaxSlider(-360, 360)] Vector2 angleBounds = new Vector2(0, 90);

    private int direction = +1;
    private Vector3 currentEulerAngles;

    float minAngle { get { return angleBounds.x; } set { angleBounds.x = value; } }
    float maxAngle { get { return angleBounds.y; } set { angleBounds.y = value; } }

    void Start()
    {
        currentEulerAngles = transform.eulerAngles;
    }

    void FixedUpdate()
    {
        currentEulerAngles += new Vector3(0, 0, direction * rotationalSpeed);

        if (currentEulerAngles.z < minAngle)
        {
            direction = +1;
            currentEulerAngles = new Vector3(0, 0, minAngle);
        }
        else if (currentEulerAngles.z > maxAngle)
        {
            direction = -1;
            currentEulerAngles = new Vector3(0, 0, maxAngle);
        }

        transform.eulerAngles = currentEulerAngles;
    }
}
