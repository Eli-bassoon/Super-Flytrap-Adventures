using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

// A utility class to load multi-scene levels
public class LevelLoader : MonoBehaviour
{
    static List<List<string>> levelScenes = new List<List<string>>
    {
        // Level 1
        new List<string> {"Intro", "Hub", "Hanging Pots", "Toxic" },
        
        // Level 2
        new List<string> {"Hub", "Light1", "Light2", "End" },
    };

    const string SCENE_PATH = "Assets/Scenes/Levels/Level";

    public static bool levelLoaded = false;

    public static void LoadLevelFade(int level)
    {
        LevelFader.instance.LoadLevel(level);
    }

    // Load a level consisting of multiple scenes. This works in both edit mode and play mode
    public static void LoadLevel(int level)
    {
        // Start loading the level
        levelLoaded = false;

        List<string> thisLevelScenes = levelScenes[level - 1];

        // Load the main scene of the level
        string firstScenePath = JoinScenePath(level, thisLevelScenes[0]);

        // Editor only
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(firstScenePath);
            GameManager.GM.SetLevel(level - 1);
        }
        else
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(firstScenePath);

            // This is black magic but it makes the level in frame correctly
            GameObject tm = GameObject.Find("Tiles");
            if (tm != null)
            {
                Selection.activeGameObject = tm;
                SceneView.FrameLastActiveSceneView();
                Selection.activeGameObject = null;

                GameObject player = GameObject.Find("Head");
                if (player != null)
                { 
                    Selection.activeGameObject = player;
                }
            }
        }

        // Game mode
#else
        // Load the scene in the game
        SceneManager.LoadScene(firstScenePath);

        // Change the level the game thinks it's on
        GameManager.GM.SetLevel(level);
#endif

        // Load the extra scenes of the level
        foreach (string sceneName in thisLevelScenes.Skip(1))
        {
            string scenePath = JoinScenePath(level, sceneName);

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            }
            else
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }
#else
            SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
#endif
        }
        levelLoaded = true;
    }

    static string JoinScenePath(int level, string sceneName)
    {
        return SCENE_PATH + level + "/" + sceneName + ".unity";
    }

    // Unity editor menu options
#if UNITY_EDITOR

    const string MULTI_SCENE_MENU = "File/Load Multi Scene Level/";
    const int START_PRIORITY = 150;

    [MenuItem(MULTI_SCENE_MENU + "Level 1", priority = START_PRIORITY + 1)]
    static void LoadLevel1()
    {
        LoadLevel(1);
    }

    [MenuItem(MULTI_SCENE_MENU + "Level 2", priority = START_PRIORITY + 2)]
    static void LoadLevel2()
    {
        LoadLevel(2);
    }

#endif
}