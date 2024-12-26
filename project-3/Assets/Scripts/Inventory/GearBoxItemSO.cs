using UnityEngine;

[CreateAssetMenu(fileName = "AmmoItemSO", menuName = "Scriptable Objects/Item/GearBox")]
public class GearBoxItemSO : ItemSO
{
    public int repairAmount;
    public float repairDuration;

    public GearBoxItemSO()
    {
        itemType = ItemType.GearBox;
    }
}
