using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    [SerializeField] public string firstLevelFilepath = "";



    public void StartGame()
    { 
        LevelLoader.LoadLevelFade(1);
    }

}
