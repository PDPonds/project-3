using UnityEngine;

public enum WeaponType
{
    CloseWeapon, RangeWeapon
}


public class WeaponItemSO : ItemSO
{
    public WeaponType weaponType;

    public WeaponItemSO()
    {
        itemType = ItemType.Weapon;
    }

}
