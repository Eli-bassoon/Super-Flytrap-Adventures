using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeYLookahead : MonoBehaviour
{
    public float waitTime = 1.5f;
    public bool ignoreY = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Timer.Register(waitTime, () =>
            {
                var vc = (CinemachineVirtualCamera)Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
                vc.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadIgnoreY = ignoreY;
            });
    }
}
