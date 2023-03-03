using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flytrap : MonoBehaviour
{

    [SerializeField] public Rigidbody2D head;
    [SerializeField] public Rigidbody2D pot;

    [SerializeField] public float pullForce = 10f;
    [SerializeField] public float maxPullDistance = 2f;

    public bool moving = false;
    public bool clamping = false;
    public bool stuckToWall = false;
    public bool mouthFull = false;

    public Vector2 mousePosition;
    public Vector2 pullDistance;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        moving = Input.GetMouseButton(0);
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }


    private void FixedUpdate()
    {
        if (moving)
        {
            Move();
        }
        else
        {
            Idle();
        }
    }



    //longer scripts below

    private void Idle()
    {
        head.drag = 0.5f;

    }


    private void Move()
    {
        head.drag = 0.1f;
        pullDistance = mousePosition - (Vector2)head.transform.position;

        if (pullDistance.magnitude > maxPullDistance)
        {
            pullDistance = pullDistance.normalized * maxPullDistance;
        }

        pullDistance *= pullForce;

        // head.AddForce(pullDistance, ForceMode2D.Impulse);
        // pot.AddForce(-pullDistance, ForceMode2D.Impulse);

        head.velocity = pullDistance;


        // Debug.DrawLine(head.transform.position, pot.transform.position);


    }



}
