using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

// A utility class to load levels
public class LevelLoader : MonoBehaviour
{
    const string SCENE_PATH = "Scenes/Levels/Level";

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

        // Load the main scene of the level
        string scenePath = SCENE_PATH + " " + level;

        // Editor only
#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(scenePath);
            GameManager.GM.SetLevel(level - 1);
        }
        else
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);

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
        SceneManager.LoadScene(scenePath);

        // Change the level the game thinks it's on
        GameManager.GM.SetLevel(level - 1);
#endif

        levelLoaded = true;
    }
}