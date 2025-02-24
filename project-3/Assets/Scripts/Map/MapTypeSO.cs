using System.Collections.Generic;
using System;
using UnityEngine;

public enum MapType
{
    Supplies, Vehicle, GasStation, Camp
}

public class MapTypeSO : ScriptableObject
{
    public string mapName;
    public Sprite mapIcon;
    public MapType mapType;

}

[Serializable]
public class Tile
{
    public GameObject TilePrefab;
    public bool isBuilding;
}