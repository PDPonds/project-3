using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    GameObject curMap;

    public MapTypeSO[] allMapType;
    [HideInInspector] public List<MapTypeSO> previousMap = new List<MapTypeSO>();

    [Header("===== Map Prefab =====")]
    [SerializeField] GameObject mapPrefab;

    [Header("===== Custom Map =====")]
    [Header("- Start Map")]
    [SerializeField] List<CustomMap> startMapTypes;
    [Header("- Objective Map")]
    [SerializeField] List<ObjectiveMapSlot> objectiveMaps;
    [Header("- End Map")]
    [SerializeField] CustomMap endMapType;


    public List<MapTypeSO> RandomMap(int count)
    {
        List<MapTypeSO> maps = new List<MapTypeSO>();

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, allMapType.Length);
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

    public CustomMap RandomStartMap()
    {
        int index = UnityEngine.Random.Range(0, startMapTypes.Count);
        return startMapTypes[index];
    }

    public bool IsObjectiveDay(int day, TimeOfDay time, out CustomMap map)
    {
        if (objectiveMaps.Count > 0)
        {
            for (int i = 0; i < objectiveMaps.Count; i++)
            {
                if (day == objectiveMaps[i].objectiveDay &&
                    time == objectiveMaps[i].objectiveTimeOfDay)
                {
                    map = objectiveMaps[i].objectiveMap;
                    return true;
                }
            }
        }

        map = null;
        return false;
    }

}

[Serializable]
public class ObjectiveMapSlot
{
    public CustomMap objectiveMap;
    public int objectiveDay;
    public TimeOfDay objectiveTimeOfDay;
}
