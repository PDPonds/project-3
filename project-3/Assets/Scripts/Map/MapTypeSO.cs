using System.Collections.Generic;
using System;
using UnityEngine;

public enum MapType
{
    Supplies, Vehicle, GasStation, Story, Camp
}

[CreateAssetMenu(fileName = "MapType", menuName = "Scriptable Objects/MapType")]
public class MapTypeSO : ScriptableObject
{
    public string mapName;
    public Sprite mapIcon;
    public MapType mapType;
    public int maxMapValue;
    [Header("===== Tile Type =====")]
    public List<Tile> allTileCanGeneratePrefab = new List<Tile>();

    public List<GameObject> GetAllTilePrefab()
    {
        List<GameObject> tilePrefabs = new List<GameObject>();
        int curTileValue = 0;
        while (curTileValue < maxMapValue)
        {
            int tileIndex = UnityEngine.Random.Range(0, allTileCanGeneratePrefab.Count);
            tilePrefabs.Add(allTileCanGeneratePrefab[tileIndex].tilePrefab);
            curTileValue += allTileCanGeneratePrefab[tileIndex].tileValue;
        }

        return tilePrefabs;
    }

}

[Serializable]
public class Tile
{
    public GameObject tilePrefab;
    public int tileValue;
}
