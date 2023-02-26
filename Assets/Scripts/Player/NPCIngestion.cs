using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class NPCIngestion : MonoBehaviour
{
    [ReadOnly] public GameObject regurgitableNPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { 
            if (regurgitableNPC == null && PlayerMovement.instance.stuckTo != null && PlayerMovement.instance.stuckTo.TryGetComponent(out FitsInMouth rbObj))
            {
                //stuckTo is the rb component of whatever we are stuck to, get game component & change active state
                if (rbObj.regurgitable)
                {
                    PlayerMovement.instance.LetGo();
                    
                    regurgitableNPC = rbObj.gameObject;
                    print(regurgitableNPC.name);
                    rbObj.transform.parent = transform;
                    regurgitableNPC.SetActive(false);
                    PlayerMovement.instance.canGrab = false;
                }
            }
            else if (regurgitableNPC != null)
            {
                regurgitableNPC.SetActive(true);
                regurgitableNPC.transform.parent = null;
                regurgitableNPC = null;
            }
        }
    }
}
