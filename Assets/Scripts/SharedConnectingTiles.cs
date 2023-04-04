using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "My Assets/Shared Connecting Tiles")]
public class SharedConnectingTiles : ScriptableObject
{
    public TileBase[] sharedConnectingTiles;
}
