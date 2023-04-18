#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileSwitcher : MonoBehaviour
{
    // Put "tileset.png" into the main Resources folder
    // Drag your rule tile into "R Tile"

    public Sprite[] spriteList;
    public AdvancedRuleTile ruleTile;

    int[] ruleOrder = { 33, 27, 34, 32, 0, 2, 16, 14, 1, 43, 44, 45, 46, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 17, 18, 19, 20, 21, 23, 24, 22, 25, 26, 28, 29, 30, 31, 35, 36, 37, 38, 39, 40, 41, 42 };

    void Start()
    {
        spriteList = Resources.LoadAll<Sprite>("tileset");
        Debug.Log("Loaded sprites");

        ruleTile.m_DefaultSprite = spriteList[17];

        for (int i = 0; i < ruleTile.m_TilingRules.Count; i++)
        {
            ruleTile.m_TilingRules[i].m_Sprites[0] = spriteList[ruleOrder[i]];
        }
        EditorUtility.SetDirty(ruleTile);
    }

    void Update()
    {
        
    }
}
#endif