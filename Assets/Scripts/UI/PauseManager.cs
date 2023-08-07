using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] GameObject speedrunTimer;

    [SerializeField] Toggle deathCounterToggle;
    [SerializeField] Toggle invertedSwingingToggle;
    [SerializeField] Toggle speedrunTimerToggle;

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
        SoundManager.SM.GetAudioSources();
        ChangeMusicVolume();
        ChangeSoundVolume();
    }

    public void Awake()
    {
        bool invertedSwingingOn = (PlayerPrefs.GetInt("invertedSwingingOn", 0) == 1) ? true : false;
        GameManager.GM.invertedSwinging = invertedSwingingOn;
        invertedSwingingToggle.SetIsOnWithoutNotify(invertedSwingingOn);

        bool deathCounterOn = (PlayerPrefs.GetInt("deathCounterOn", 0) == 1) ? true : false;
        deathCounter.SetActive(deathCounterOn);
        deathCounterToggle.SetIsOnWithoutNotify(deathCounterOn);

        bool speedrunTimerOn = (PlayerPrefs.GetInt("speedrunTimerOn", 0) == 1) ? true : false;
        speedrunTimer.SetActive(speedrunTimerOn);
        speedrunTimerToggle.SetIsOnWithoutNotify(speedrunTimerOn);
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

        // Reset deaths and times
        deathCounter.GetComponent<DeathCounter>().deaths = 0;
        SpeedrunManager.instance.ResetTimes();
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

    public void UpdateDeathCounter(bool isOn)
    {
        deathCounter.SetActive(isOn);
        PlayerPrefs.SetInt("deathCounterOn", isOn ? 1 : 0);
    }

    public void UpdateInvertedSwinging(bool isOn)
    {
        GameManager.GM.invertedSwinging = isOn;
        PlayerPrefs.SetInt("invertedSwingingOn", isOn ? 1 : 0);
    }

    public void UpdateSpeedrunTimer(bool isOn)
    {
        speedrunTimer.SetActive(isOn);
        PlayerPrefs.SetInt("speedrunTimerOn", isOn ? 1 : 0);
    }
}
