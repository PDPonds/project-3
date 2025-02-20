using UnityEngine;

public enum ItemType
{
    Weapon, GearBox, Gastank, Food, Drink, Heal, Ammo , General
}

public class ItemSO : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    [Header("==== Weigth =====")]
    public float itemWeigth;
    [Header("==== Cost =====")]
    public Vector2Int Min_Max_ItemCost;
    [Header("==== Prefab =====")]
    public GameObject itemOnHandPrefab;
    [Header("===== Stackable =====")]
    public bool itemStackable;
    [Header("==== Attack =====")]
    public int damage;
    public float attackDelay;
    public float attackRange;
    [Header("==== Type =====")]
    public ItemType itemType;


    public int GetRandomCost()
    {
        int cost = Random.Range(Min_Max_ItemCost.x, Min_Max_ItemCost.y);
        return cost;
    }

}