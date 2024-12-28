using UnityEngine;

public enum ItemType
{
    Weapon, GearBox, Gastank, Food, Drink, Heal, Ammo
}

public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public float itemWeigth;
    public Vector2Int Min_Max_ItemCost;
    public GameObject itemPrefab;
    public bool itemStackable;
    public bool itemCanHoldInHand;
    public ItemType itemType;


    public int GetRandomCost()
    {
        int cost = Random.Range(Min_Max_ItemCost.x, Min_Max_ItemCost.y);
        return cost;
    }

}