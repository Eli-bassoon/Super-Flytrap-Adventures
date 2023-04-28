using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomController : MonoBehaviour
{
    [SerializeField] Transform allObjectParent;
    [SerializeField] float approachDelay = 1f;

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
            float maxX = startX + progress;
            foreach (var obj in toggleObjects)
            {
                if (obj.transform.position.x < maxX)
                {
                    obj.TurnOff();
                }
            }

            print($"Progress: {progress}");

            yield return new WaitForSeconds(approachDelay);
        }
    }
}
