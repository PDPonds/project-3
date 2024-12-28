using UnityEngine;

[CreateAssetMenu(fileName = "HealItemSO", menuName = "Scriptable Objects/Item/Heal")]
public class HealItemSO : ItemSO
{
    public int healAmount;
    public float healDuration;

    public HealItemSO()
    {
        itemStackable = true;
        itemCanHoldInHand = true;
        itemType = ItemType.Heal;
    }
}
