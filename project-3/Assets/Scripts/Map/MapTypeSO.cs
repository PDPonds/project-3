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
}
