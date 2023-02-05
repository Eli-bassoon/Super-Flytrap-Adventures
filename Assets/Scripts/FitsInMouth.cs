using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this component to anything that can be held in the player's mouth
public class FitsInMouth : MonoBehaviour
{
    [Tooltip("Whether the object should be moved to the center of the player's mouth.")] public bool centerMe = true;
}
