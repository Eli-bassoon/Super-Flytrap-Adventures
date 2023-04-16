using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager GM;

    // Variables
    [HideInInspector] public bool gameIsRunning = true;

    // Runs before a scene gets loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadGM()
    {
        GameObject GM = Instantiate(Resources.Load("Prefabs/GameManager")) as GameObject;
        DontDestroyOnLoad(GM);
    }

    private void Awake()
    {
        //Application.targetFrameRate = 30;

        if (GM == null)
        {
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // It is save to remove listeners even if they
        // didn't exist so far.
        // This makes sure it is added only once
        SceneManager.sceneLoaded -= OnSceneLoaded;
        // Add the listener to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public void Exit()
    {
        GetComponent<ExitGame>().Exit();
    }

    public void ChangeVolume(float volume)
    {
        GetComponent<AudioSource>().volume = volume;
    }
}