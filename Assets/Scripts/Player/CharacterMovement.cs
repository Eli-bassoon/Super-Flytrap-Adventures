using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 mousePosition;
    private bool mousePressed;
    private bool mouseButtonUp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePressed = false;
        // Presses the mouse
        if (Input.GetMouseButtonDown(0))
        {
            controller.ExtendTongue();
            controller.canGrab = true;
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
        // Move our character
        if (mousePressed && controller.canGrab)
        {
            controller.TestForWall(mousePosition);
            controller.Move(mousePosition);
        }
        if (mouseButtonUp)
        {
            controller.LetGo();
            mouseButtonUp = false;
            controller.canGrab = true;
        }
    }
}
