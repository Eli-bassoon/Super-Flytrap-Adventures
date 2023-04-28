using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static bool canPause = true;
    public static bool GameIsPaused = false;
    public static float pauseTime = .5f;
    [Scene] public string mainMenuScene;

    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public Fader fade;

    [SerializeField] GameObject deathCounter;
    [SerializeField] Toggle deathCounterToggle;

    [SerializeField] Toggle invertedSwingingToggle;

    public Slider musicSlider;
    public Slider soundSlider;

    public void Start()
    {
        //Unpause();
        pauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;

        // Load & enforce player prefs
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("soundVolume", 1f);
        ChangeMusicVolume();
        ChangeSoundVolume();
    }

    public void Awake()
    {
        bool deathCounterOn = (PlayerPrefs.GetInt("deathCounterOn", 0) == 1) ? true : false;
        deathCounter.SetActive(deathCounterOn);
        deathCounterToggle.SetIsOnWithoutNotify(deathCounterOn);
    }

    public void Pause()
    {
        if (!canPause) return;

        PlayerMovement.instance.LetGo();
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        fade.FadeIn(pauseTime);
    }

    public void Unpause()
    {
        if (!canPause) return;

        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        fade.FadeOut(pauseTime);
        PlayerPrefs.Save();
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Unpause();
        } else
        {
            Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
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
        deathCounter.SetActive(false);
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
        PlayerPrefs.SetFloat("musicVolume", vol);
    }

    public void ChangeSoundVolume()
    {
        float vol = soundSlider.value;
        SoundManager.SM.ChangeVolume(vol);
        PlayerPrefs.SetFloat("soundVolume", vol);
    }

    public void UpdateDeathCounter()
    {
        if (deathCounterToggle.isOn)
        {
            deathCounter.SetActive(true);
        }
        else deathCounter.SetActive(false);
        PlayerPrefs.SetInt("deathCounterOn", deathCounterToggle.isOn ? 1 : 0);
    }

    public void UpdateInvertedSwinging()
    {
        GameManager.GM.invertedSwinging = invertedSwingingToggle.isOn;
        //print("inverted swinging is" +  invertedSwingingToggle.isOn);
    }
}
