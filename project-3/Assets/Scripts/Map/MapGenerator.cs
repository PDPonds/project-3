using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    GameObject curMap;

    public MapTypeSO[] allMapType;
    [HideInInspector] public List<MapTypeSO> previousMap = new List<MapTypeSO>();

    [Header("===== Map Prefab =====")]
    [SerializeField] GameObject mapPrefab;

    public List<MapTypeSO> RandomMap(int count)
    {
        List<MapTypeSO> maps = new List<MapTypeSO>();

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, allMapType.Length);
            if (!maps.Contains(allMapType[index]))
            {
                maps.Add(allMapType[index]);
            }
            else
            {
                i--;
            }
        }

        return maps;
    }


    public void GenerateMap(MapTypeSO mapTypeSO)
    {
        if (curMap != null) Destroy(curMap);

        GameObject mapObj = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        Map map = mapObj.GetComponent<Map>();
        map.Setup(mapTypeSO);
        curMap = mapObj;
    }

}
