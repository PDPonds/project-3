using UnityEngine;

public enum ItemType
{
    Weapon, GearBox, Fueltank, Food, Drink, Heal, Ammo
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public float itemWeigth;
    public int itemMaxStack;
    public Vector2Int Min_Max_ItemCost;
    public GameObject itemPrefab;
    public ItemType itemType;

    public int GetRandomCost()
    {
        int cost = Random.Range(Min_Max_ItemCost.x, Min_Max_ItemCost.y);
        return cost;
    }

}