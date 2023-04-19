using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public static float pauseTime = .5f;
    [Scene] public string mainMenuScene;

    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public Fader fade;

    public Slider musicSlider;
    public Slider soundSlider;

    public void Start()
    {
        Unpause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        fade.FadeIn(pauseTime);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        fade.FadeOut(pauseTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetToCheckpoint()
    {
        //clytie plz help  D:
        DamageHandler.instance.Respawn(); // is this what u mean
        Unpause();
    }
    public void GoToMainMenu()
    {
        Unpause();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Unpause();
        GameManager.GM.Exit();
    }

    public void ChangeMusicVolume()
    {
       float vol = musicSlider.value;
       MusicManager.MM.SetMusicVolume(vol);
    }
}
