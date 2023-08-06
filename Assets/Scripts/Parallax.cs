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
        playerStartPos = PlayerMovement.instance.transform.position;
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 playerDelt = (Vector2)PlayerMovement.instance.transform.position - playerStartPos;
        transform.position = startPos - playerDelt * parallaxFactor;
    }
}
