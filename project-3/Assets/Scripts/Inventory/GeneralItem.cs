using UnityEngine;

[CreateAssetMenu(fileName = "GeneralItem", menuName = "Scriptable Objects/Item/GeneralItem")]
public class GeneralItem : ItemSO
{
    public GeneralItem()
    {
        itemStackable = true;
        itemType = ItemType.General;
    }
}
