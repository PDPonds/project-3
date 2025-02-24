using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomMapType", menuName = "Scriptable Objects/MapType/CustomMap")]
public class CustomMap : MapTypeSO
{
    [Header("===== Road =====")]
    public List<CustomRoad> road = new List<CustomRoad>();

    [Header("===== Tile Type =====")]
    public List<CustomTileSet> customTileSets = new List<CustomTileSet>();
}

[Serializable]
public class CustomTileSet
{
    public Tile tile;
    public Vector2Int position;

}

[Serializable]
public class CustomRoad
{
    [Header("===== General ======")]
    public bool isRoad;
    public float YRotation;
    public bool isFourWay;
    [Header("===== Spawn Point ======")]
    public bool isSpawnPoint;
    [Header("===== Exit Point ======")]
    public bool exitPoint;
    public float exitPointRotation;
    public Vector2Int position;
}
