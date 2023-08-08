using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxFactor = 0.02f;

    Vector2 playerStartPos;
    Vector2 startPos;

    void Start()
    {
        playerStartPos = Camera.main.transform.position;
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 playerDelt = (Vector2)Camera.main.transform.position - playerStartPos;
        transform.position = startPos - playerDelt * parallaxFactor;
    }
}
