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
    [Range(1, 6)] public int maxMapValue;
    [Header("===== Tile Type =====")]
    public List<GameObject> AllBuildingTileCanInit = new List<GameObject>();
    public List<GameObject> AllNoneBuildingTileCanInit = new List<GameObject>();

    public List<Tile> GetAllTilePrefab(int buildingCount, int noneBuildingCount)
    {
        List<Tile> tilePrefabs = new List<Tile>();
        if (buildingCount > 0)
        {
            int count = UnityEngine.Random.Range(1, buildingCount);
            for (int i = 0; i < count; i++)
            {
                int rand = UnityEngine.Random.Range(0, AllBuildingTileCanInit.Count);
                GameObject go = AllBuildingTileCanInit[rand];
                Tile tile = new Tile();
                tile.TilePrefab = go;
                tile.isBuilding = true;
                tilePrefabs.Add(tile);
            }
        }

        if (noneBuildingCount > 0)
        {
            int count = UnityEngine.Random.Range(1, noneBuildingCount);
            for (int i = 0; i < count; i++)
            {
                int rand = UnityEngine.Random.Range(0, AllNoneBuildingTileCanInit.Count);
                GameObject go = AllNoneBuildingTileCanInit[rand];
                Tile tile = new Tile();
                tile.TilePrefab = go;
                tile.isBuilding = false;
                tilePrefabs.Add(tile);
            }
        }

        return tilePrefabs;
    }

}

[Serializable]
public class Tile
{
    public GameObject TilePrefab;
    public bool isBuilding;
}