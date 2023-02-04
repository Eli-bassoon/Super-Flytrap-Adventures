using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CharacterController : MonoBehaviour
{
    enum MouthStates
    {
        Closed,
        Open,
        Grabbing,
    }
    public static CharacterController instance;
    public static int mouthStateAnim = Animator.StringToHash("mouthState");

    [SerializeField] private float headPullForce = 8f;
    [SerializeField] private float headPullVelocity = 15f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [Range(0, 8f)] [SerializeField] private float maxNeckLength = 3.4f;
    [SerializeField] private LayerMask grabbableLayers;
    [SerializeField] private float mouseChatterThreshold = 0.5f;
    [Range(-0.5f, 0)] [SerializeField] private float comOffset = -0.2f;

    public Rigidbody2D flowerpot;
    public Rigidbody2D tongue;

    private CircleCollider2D tongueCollider;
    private FixedJoint2D tongueFixedJoint;
    private SpringJoint2D springJoint;
    private FixedJoint2D fixedJoint;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector3 mousePosition;
    private bool mousePressed;
    private bool mouseButtonUp;

    private Vector3 zeroVelocity = new Vector3(0, 0, 0);
    private float extraWallCheckRadius = 0.2f;
    private float attachedSpringFreq = 2f;
    private float freeSpringFreq = 0.7f;
    private float maxTongueLength;
    private float distanceToPot = 0f;
    private float behindTongueChatterThreshold = 0.2f;
    private Rigidbody2D prevStuckTo = null;
    [HideInInspector] public Rigidbody2D stuckTo = null;
    [HideInInspector] public Collider2D stuckToCollider = null;
    [HideInInspector] public bool stuck = false;
    private bool mouthFull = false;
    private bool retractingTongue = false;
    private Vector2 retractingDirection;
    [HideInInspector] public bool canGrab = true;
    private Coroutine tongueCoroutine;

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
            springJoint.frequency = freeSpringFreq;
        }

        // Mouth move to center
        if (mouthFull && stuckToCollider.GetComponent<FitsInMouth>().centerMe)
        {
            stuckToCollider.transform.position = transform.position;
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
            springJoint.frequency = attachedSpringFreq;
            
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
                }
                else
                {
                    movingRigidbody.AddForce(targetForce);
                }
                flowerpot.AddForce(-targetForce);
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
            if (Vector2.Distance(tongue.position, rb.position) < 0.2f)
            {
                PointAt(retractingDirection);

                if (stuckToCollider.gameObject.TryGetComponent(out FitsInMouth fitsInMouth))
                {
                    mouthFull = true;
                    if (fitsInMouth.centerMe)
                    {
                        stuckToCollider.transform.position = transform.position;
                    }
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
        prevStuckTo = stuckTo;

        // Stuck to a rigid body
        if (stuckTo != null)
        {

        }

        stuck = false;
        mouthFull = false;

        stuckTo = null;
        stuckToCollider = null;

        springJoint.enabled = true;
        springJoint.frequency = freeSpringFreq;
        fixedJoint.enabled = false;
        fixedJoint.connectedBody = null;
        rb.centerOfMass = new Vector2(comOffset, 0);

        DisableTongue();
        anim.SetInteger(mouthStateAnim, (int)MouthStates.Closed);
    }

    // The tongue is entirely in the mouth
    void DisableTongue()
    {
        retractingTongue = false;

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