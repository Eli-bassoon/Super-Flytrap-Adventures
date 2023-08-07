using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
