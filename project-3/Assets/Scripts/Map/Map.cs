using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapTypeSO curMapType;

    [Header("===== Generate Map Condition =====")]
    public List<Transform> tileSpawnPoint = new List<Transform>();
    [Header("===== Spawn And Exit Point =====")]
    public Transform spawnPoint;
    public Transform exitPoint;


    public void Setup(MapTypeSO mapTypeSO)
    {
        curMapType = mapTypeSO;
        List<GameObject> allTilePrefabs = mapTypeSO.GetAllTilePrefab();

    }
}
