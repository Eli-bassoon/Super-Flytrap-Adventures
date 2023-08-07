using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomController : MonoBehaviour
{
    [SerializeField] Transform allObjectParent;
    [SerializeField] float approachDelay = 1f;
    [SerializeField] int playerSlack = 9;

    List<Toggleable> toggleObjects;

    int progress;
    int maxProgress;
    float startX;

    // Start is called before the first frame update
    void Start()
    {
        startX = Mathf.Floor(transform.position.x);

        toggleObjects = new List<Toggleable>(allObjectParent.GetComponentsInChildren<Toggleable>());

        // Get the maximum progress
        float maxX = 0;
        foreach (var obj in toggleObjects)
        {
            if (obj.transform.position.x > maxX)
            {
                maxX = obj.transform.position.x;
            }
        }

        maxProgress = Mathf.CeilToInt(maxX);

        //StartCoroutine(DoomWall());
    }

    void Update()
    {
        
    }

    public void StartDoomWall()
    {
        StartCoroutine(DoomWall());
    }

    public void ResetDoomWall()
    {
        StopAllCoroutines();
        progress = 0;

        foreach (var obj in toggleObjects)
        {
            obj.TurnOn();
        }
    }

    // Runs the wall of doom
    IEnumerator DoomWall()
    {
        yield return new WaitForEndOfFrame();

        while (progress < maxProgress)
        {
            // Turn off everything that falls too far behind
            progress++;

            // Makes sure the progress is closeish to the player
            int playerBoundary = Mathf.FloorToInt(PlayerMovement.instance.transform.position.x - startX - playerSlack);
            progress = Mathf.Clamp(progress, playerBoundary, maxProgress);

            float maxX = startX + progress;
            foreach (var obj in toggleObjects)
            {
                if (obj.transform.position.x < maxX)
                {
                    if (obj.IsOn) obj.TurnOff();
                }
            }

            print($"Progress: {progress}");

            yield return new WaitForSeconds(approachDelay);
        }
    }
}
