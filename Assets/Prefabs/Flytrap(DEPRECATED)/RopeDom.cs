using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeDom : MonoBehaviour
{
    [SerializeField] public int numLinks = 10;
    [SerializeField] public Rigidbody2D head;
    [SerializeField] public Rigidbody2D pot;
    [SerializeField] public GameObject prefabRopeSeg;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
    }


    void GenerateRope()
    {
        // start w/ pot
        Rigidbody2D prevBod = pot;

        //create and connect links
        for (int i = 0; i < numLinks; i++)
        {
            // make new segment, add it to previous link/pot
            GameObject newSeg = Instantiate(prefabRopeSeg);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D newHJ = newSeg.GetComponent<HingeJoint2D>();
            newHJ.connectedBody = prevBod;

            // save new seg as prev seg for next iteration
            prevBod = newSeg.GetComponent<Rigidbody2D>();
        }

        //connect last seg to head 
        head.GetComponent<HingeJoint2D>().connectedBody = prevBod;


    }

}
