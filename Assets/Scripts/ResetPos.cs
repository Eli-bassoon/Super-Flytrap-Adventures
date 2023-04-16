using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    Vector2 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void MoveToStartPos()
    {
        transform.position = startPos;
    }
}
