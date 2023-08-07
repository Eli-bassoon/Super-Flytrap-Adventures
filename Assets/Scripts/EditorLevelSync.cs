using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorLevelSync : MonoBehaviour
{
    [SerializeField] int thisLevel;

    void Start()
    {
#if UNITY_EDITOR
        GameManager.GM.SetLevel(thisLevel);
#endif
    }
}
