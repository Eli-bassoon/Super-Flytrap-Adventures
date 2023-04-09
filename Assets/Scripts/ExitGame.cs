using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] float timeToQuit = 1f;

    Timer escTimer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escTimer = Timer.Register(timeToQuit, () => Exit(), useRealTime: true);
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            escTimer.Cancel();
        }
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
