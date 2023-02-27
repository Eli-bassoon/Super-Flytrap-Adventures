using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum MouthStates
    {
        Closed,
        Open,
        Grabbing,
    }
    public static PlayerMovement instance;
    public static int mouthStateAnim = Animator.StringToHash("mouthState");

    [SerializeField] float headPullForce = 8f;
    [SerializeField] float headPullVelocity = 15f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = 0.05f;
    [Range(0, 8f)] [SerializeField] float maxNeckLength = 3.4f;
    [SerializeField] LayerMask grabbableLayers;
    [SerializeField] float mouseChatterThreshold = 0.5f;
    [SerializeField] float extraWallCheckRadius = 0.2f;
    [Range(-0.5f, 0)] [SerializeField] float comOffset = -0.2f;
    [SerializeField] float targetFreeDist = 1f;
    [SerializeField] float freeForceP = 1;
    [SerializeField] float freeForceD = 0;
    [SerializeField] float freeAngleP = 1;
    [SerializeField] float freeAngleD = 0;
    [SerializeField] Vector2 flowerpotVBias;

    public Rigidbody2D flowerpot;
    public Rigidbody2D tongue;
    public GameObject rightMouthHalf;

    CircleCollider2D tongueCollider;
    FixedJoint2D tongueFixedJoint;
    SpringJoint2D springJoint;
    FixedJoint2D fixedJoint;
    Rigidbody2D rb;
    Animator anim;

    Vector3 mousePosition;
    bool mousePressed;
    bool mouseButtonUp;

    Vector3 zeroVelocity = new Vector3(0, 0, 0);
    float maxTongueLength;
    float distanceToPot = 0f;
    float behindTongueChatterThreshold = 0.2f;

    float restingAngle = -90;
    Vector2 freePosDelt;
    float freeAngleDelt;

    [HideInInspector] public Rigidbody2D stuckTo = null;
    [HideInInspector] public Collider2D stuckToCollider = null;
    [ReadOnly] public bool stuck = false;
    [ReadOnly] public bool mouthFull = false;
    [ReadOnly] public bool retractingTongue = false;
    [HideInInspector] public bool canGrab = true;

    Vector2 retractingDirection;
    Coroutine tongueCoroutine;

    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        fixedJoint = GetComponent<FixedJoint2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        tongueCollider = tongue.GetComponent<CircleCollider2D>();
        tongueFixedJoint = tongue.GetComponent<FixedJoint2D>();

        maxTongueLength = tongue.GetComponent<DistanceJoint2D>().distance;

        // Unlock flowerpot from this
        flowerpot.transform.parent = null;
    }

    void Update()
    {
        mousePressed = false;
        // Presses the mouse
        if (Input.GetMouseButtonDown(0))
        {
            ExtendTongue();
            canGrab = true;
        }
        // Gets a vector to the mouse's position in world if pressed down
        if (Input.GetMouseButton(0))
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePressed = true;
            if (!stuck || mouthFull) ChangeRestingAngle();
        }
        // If we let go of the mouse button, see if we get launched
        if (Input.GetMouseButtonUp(0))
        {
            mouseButtonUp = true;
        }
    }

    void FixedUpdate()
    {
        distanceToPot = Vector3.Distance(transform.position, flowerpot.position);

        if (!stuck)
        {
            MoveToRest();
        }

        // Move our character
        if (mousePressed && canGrab)
        {
            TestForWall(mousePosition);
            Move(mousePosition);
        }
        if (mouseButtonUp)
        {
            LetGo();
            mouseButtonUp = false;
            canGrab = true;
        }
    }

    // Changes the head's direction to left/right depending on where was last clicked
    void ChangeRestingAngle()
    {
        SpriteRenderer rightMouthHalfRenderer = rightMouthHalf.GetComponent<SpriteRenderer>();

        if (((Vector2)mousePosition - flowerpot.position).x < 0)
        {
            restingAngle = +90;
            rightMouthHalfRenderer.sortingOrder = 1;
        }
        else
        {
            restingAngle = -90;
            rightMouthHalfRenderer.sortingOrder = 0;
        }
    }

    // Makes the jaws look at a certain position
    void PointAt(Vector3 pointDirection, bool includeTongue = false)
    {
        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);
        if (includeTongue)
        {
            tongue.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    // The head moves towards the mouse, normally
    public void Move(Vector3 mousePosition)
    {
        // Move the pot by making spring stronger
        if (stuck)
        {
            springJoint.enabled = true;
            
            // Tests for falling off
            if (!stuckToCollider.enabled)
            {
                LetGo();
                canGrab = false;
            }
        }
        // Move the head towards the mouse
        if ((!stuck || mouthFull) && canGrab)
        {
            Rigidbody2D movingRigidbody;
            // Determine which object to move
            if (mouthFull)
            {
                movingRigidbody = rb;
            }
            else
            {
                movingRigidbody = tongue;
            }
            springJoint.enabled = false;

            Vector2 moveDirection = mousePosition - movingRigidbody.transform.position;
            float distanceToMouse = moveDirection.magnitude;

            Vector3 targetForce = moveDirection.normalized * headPullForce;
            Vector3 targetVelocity = moveDirection.normalized * headPullVelocity;

            bool changeAngleToMouse = false;
            // If we're too far, apply a force so the neck stretches towards the mouse
            if (distanceToPot >= maxNeckLength)
            {
                if (mouthFull)
                {
                    stuckTo.AddForce(targetForce);
                    flowerpot.AddForce(-targetForce);
                }
                else
                {
                    movingRigidbody.AddForce(targetForce);
                }
                changeAngleToMouse = true;
            }
            // If we're inside the head's free range of motion, change the velocity to go towards the mouse
            else if (distanceToMouse >= mouseChatterThreshold)
            {
                movingRigidbody.velocity = Vector3.SmoothDamp(movingRigidbody.velocity, targetVelocity, ref zeroVelocity, movementSmoothing);
                changeAngleToMouse = true;
            }
            // We're very close to the mouse, so to reduce chatter we decrease the velocity we apply
            else
            {
                targetVelocity = moveDirection.normalized * headPullVelocity * (distanceToMouse / mouseChatterThreshold);
                movingRigidbody.velocity = Vector3.SmoothDamp(movingRigidbody.velocity, targetVelocity, ref zeroVelocity, movementSmoothing);
                if (!mouthFull)
                {
                    // Tries to move the mouth just behind the tongue
                    float tongueSlack = 0.95f;
                    Vector2 flowerpotToTongue = tongue.position - flowerpot.position;

                    Vector2 targetMouthPosition;
                    // Mouth should be about a max tongue length behind the tongue
                    if (flowerpotToTongue.magnitude >= maxTongueLength)
                    {
                        targetMouthPosition = tongue.position - flowerpotToTongue.normalized * maxTongueLength * tongueSlack; 
                    }
                    // Mouth should be by the flowerpot
                    else
                    {
                        targetMouthPosition = flowerpot.position;
                    }

                    float distanceToTargetPosition = Vector2.Distance(targetMouthPosition, rb.position);
                    // Moves to the position
                    Vector2 mouthMoveDirection = targetMouthPosition - rb.position;
                    Vector2 targetMouthVelocity = mouthMoveDirection.normalized * headPullVelocity * Mathf.Min(1, distanceToTargetPosition / behindTongueChatterThreshold);
                    rb.velocity = Vector3.SmoothDamp(rb.velocity, targetMouthVelocity, ref zeroVelocity, movementSmoothing);

                    // Resets angular velocities
                    rb.angularVelocity = 0;
                    tongue.angularVelocity = 0;
                }
            }

            Vector3 pointDirection;
            // Angle towards mouse
            if (changeAngleToMouse)
            {
                pointDirection = tongue.position - rb.position;
            }
            // To reduce chatter, we point the head towards the pot if we're close to the mouse
            else
            {
                pointDirection = tongue.position - flowerpot.position;
            }
            PointAt(pointDirection, includeTongue: true);
        }
    }

    // Gets changes since previous frame for PD control
    void GetFreeDelt()
    {
        Vector2 targetPos = flowerpot.position + Vector2.up*targetFreeDist + flowerpot.velocity*flowerpotVBias;
        freePosDelt = targetPos - rb.position;

        freeAngleDelt = restingAngle - rb.rotation;
    }
    
    // When not grabbing, tries to move the head to a resting position
    public void MoveToRest()
    {
        // Get values
        Vector2 prevFreePosDelt = freePosDelt;
        float prevFreeAngleDelt = freeAngleDelt;
        GetFreeDelt();

        // Force PD control
        Vector2 appliedForceP = freePosDelt * freeForceP;
        Vector2 appliedForceD = (freePosDelt - prevFreePosDelt) * freeForceD;
        Vector2 appliedForce = appliedForceP + appliedForceD;
        rb.AddForce(appliedForce);

        // Angle PD control
        float appliedAngleP = freeAngleDelt * freeAngleP;
        float appliedAngleD = (freeAngleDelt - prevFreeAngleDelt) * freeAngleD;
        float newOmega = appliedAngleP + appliedAngleD;
        rb.angularVelocity = newOmega;
    }

    // Sticks to a wall
    public void TestForWall(Vector3 mousePosition)
    {
        // If not already stuck, checks if we're aiming at a wall and close
        if (!stuck && canGrab)
        {
            Vector2 moveDirection = mousePosition - tongue.transform.position;
            // Tongue test for wall
            RaycastHit2D[] tongueHits = Physics2D.RaycastAll(tongue.position, moveDirection,
                tongueCollider.radius + extraWallCheckRadius, grabbableLayers);
            foreach (RaycastHit2D hit in tongueHits)
            {
                if (hit.collider != null && hit.collider.gameObject != gameObject)
                {
                    // Don't grab onto //

                    // Not grabbable, so ignore
                    if (hit.collider.gameObject.CompareTag("Not Grabbable"))
                    {
                        continue;
                    }

                    // Eating something
                    if (hit.collider.gameObject.TryGetComponent(out Enemy enemy))
                    {
                        if (enemy.edible)
                        {
                            Destroy(hit.collider.gameObject);
                        }
                        continue;
                    }

                    // Grab onto //

                    // Sticking to something
                    retractingTongue = true;
                    retractingDirection = moveDirection;
                    stuck = true;
                    stuckToCollider = hit.collider;

                    tongue.position = hit.point;
                    tongueCollider.enabled = false;
                    tongueFixedJoint.enabled = true;
                    tongueFixedJoint.autoConfigureConnectedAnchor = true;

                    // Hit a rigid body
                    if (hit.rigidbody != null)
                    {
                        tongueFixedJoint.connectedBody = hit.rigidbody;
                        tongueFixedJoint.autoConfigureConnectedAnchor = false;
                        stuckTo = hit.collider.attachedRigidbody;
                    }

                    return;
                }
            }
        }
        // Moves towards the tongue if retracting, and sticks to wall
        if (retractingTongue)
        {
            // Move towards the tongue
            Vector2 moveDirection = tongue.transform.position - transform.position;
            Vector2 targetVelocity = moveDirection.normalized * headPullVelocity;
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVelocity, movementSmoothing);

            // Fully retract the tongue
            if (Vector2.Distance(tongue.position, rb.position) < 0.1f)
            {
                PointAt(retractingDirection);

                // Center in mouth
                if (stuckTo != null && stuckTo.CompareTag("Center in Mouth"))
                {
                    stuckTo.excludeLayers = LayerMask.GetMask(new string[] { "Player" });
                    rb.position = stuckTo.position;
                }

                // Thing we're stuck to fits in mouth
                if (stuckToCollider.gameObject.TryGetComponent(out FitsInMouth _))
                {
                    stuckTo.excludeLayers = LayerMask.GetMask(new string[] {"Player"});
                    mouthFull = true;
                    fixedJoint.enabled = true;
                    fixedJoint.autoConfigureConnectedAnchor = false;
                    if (tongueFixedJoint.connectedBody != null)
                    {
                        fixedJoint.connectedBody = tongueFixedJoint.connectedBody;
                        fixedJoint.connectedAnchor = Vector2.zero;
                    }
                }
                else
                {
                    fixedJoint.enabled = true;
                    fixedJoint.autoConfigureConnectedAnchor = true;

                    if (tongueFixedJoint.connectedBody != null)
                    {
                        fixedJoint.connectedBody = tongueFixedJoint.connectedBody;
                        fixedJoint.autoConfigureConnectedAnchor = false;
                    }
                }

                // Broadcast that it has stuck to an object
                if (stuckToCollider.TryGetComponent(out IGrabHandler grabHandler))
                {
                    grabHandler.OnGrab();
                }

                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(retractingDirection.y, retractingDirection.x) * Mathf.Rad2Deg - 90);
                DisableTongue();
                anim.SetInteger(mouthStateAnim, (int)MouthStates.Grabbing);
            }
            else
            {
                PointAt(retractingDirection);
            }
        }
    }

    // We just let go of something
    public void LetGo()
    {
        // Stuck to a rigid body
        if (stuckTo != null)
        {
            if (stuckTo.TryGetComponent(out IGrabHandler grabHandler))
            {
                grabHandler.OnRelease();
            }
            if (mouthFull || stuckTo.CompareTag("Center in Mouth"))
            {
                stuckTo.excludeLayers = new LayerMask();
            }
        }

        stuck = false;
        mouthFull = false;

        stuckTo = null;
        stuckToCollider = null;

        springJoint.enabled = false;
        fixedJoint.enabled = false;
        fixedJoint.connectedBody = null;

        DisableTongue();
        GetFreeDelt();
        anim.SetInteger(mouthStateAnim, (int)MouthStates.Closed);
    }

    // The tongue is entirely in the mouth
    void DisableTongue()
    {
        retractingTongue = false;

        tongueCollider.enabled = true;
        tongueFixedJoint.enabled = false;
        tongueFixedJoint.connectedBody = null;

        if (tongueCoroutine != null)
        {
            StopCoroutine(tongueCoroutine);
        }
        tongueCoroutine = StartCoroutine(RetractTongue());
    }

    // Lets the tongue be free from the mouth
    public void ExtendTongue()
    {
        tongue.transform.parent = null;
        tongue.position = transform.position;
        tongue.gameObject.SetActive(true);

        if (tongueCoroutine != null)
        {
            StopCoroutine(tongueCoroutine);
        }

        rb.centerOfMass = new Vector2(0, 0);

        anim.SetInteger(mouthStateAnim, (int)MouthStates.Open);
    }

    // Gradually bring the tongue back into the mouth
    IEnumerator RetractTongue()
    {
        float lerpStep = 0.1f;
        int steps = 10;
        for (int i = 0; i < steps; i++)
        {
            tongue.transform.position = Vector2.Lerp(tongue.transform.position, transform.position, lerpStep);
            yield return null;
        }

        // Disables the tongue
        tongue.transform.position = transform.position;
        tongue.transform.parent = transform;
        tongue.gameObject.SetActive(false);
    }
}