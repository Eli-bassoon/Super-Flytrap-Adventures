using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) { 
            if (PlayerMovement.instance.stuckTo.TryGetComponent(out FitsInMouth obj))
            {
                //stuckTo is the rb component, get game component from it & disable
            }
        }
    }
}
