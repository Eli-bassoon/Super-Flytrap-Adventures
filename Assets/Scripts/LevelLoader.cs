using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    static List<List<string>> levelScenes = new List<List<string>>
    {
        // Level 1
        new List<string> {"Hub", "Intro", "Sunlight"},
        
        // Level 2
        new List<string> {"Hub", "Light1"},
    };

    const string SCENE_PATH = "Assets/Scenes/Levels/Level";

    static void LoadLevel(int level)
    {
        List<string> thisLevelScenes = levelScenes[level - 1];

        string firstScenePath = JoinScenePath(level, thisLevelScenes[0]);

#if UNITY_EDITOR
        EditorSceneManager.OpenScene(firstScenePath);
#else
        SceneManager.LoadScene(firstScenePath);
#endif

        foreach (string sceneName in thisLevelScenes.Skip(1))
        {
            string scenePath = JoinScenePath(level, sceneName);
#if UNITY_EDITOR
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
#else
            SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
#endif
        }
    }

    static string JoinScenePath(int level, string sceneName)
    {
        return SCENE_PATH + level + "/" + sceneName + ".unity";
    }

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