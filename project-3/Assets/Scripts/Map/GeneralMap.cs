using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralMap", menuName = "Scriptable Objects/MapType/GeneralMap")]
public class GeneralMap : MapTypeSO
{
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
