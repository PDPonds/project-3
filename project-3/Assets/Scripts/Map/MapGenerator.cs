
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    public MapTypeSO[] allMapType;
    [HideInInspector] public List<MapTypeSO> previousMap = new List<MapTypeSO>();

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

}
