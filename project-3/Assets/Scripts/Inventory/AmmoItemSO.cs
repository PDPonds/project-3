using UnityEngine;

[CreateAssetMenu(fileName = "AmmoItemSO", menuName = "Scriptable Objects/Item/Ammo")]
public class AmmoItemSO : ItemSO
{
    public AmmoItemSO()
    {
        itemStackable = true;
        itemType = ItemType.Ammo;
    }
}
