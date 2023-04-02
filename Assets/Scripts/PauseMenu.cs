using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] float pauseTime = 0.5f;

    [SerializeField] SpriteFader fade;

    bool paused = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        paused = !paused;

        // Pause the game
        if (paused)
        {
            fade.FadeIn(pauseTime);
            Time.timeScale = 0;
        }
        // Unpause the game
        else
        {
            fade.FadeOut(pauseTime);
            Time.timeScale = 1;
        }
    }
}
