using UnityEngine;

public enum WeaponType
{
    CloseWeapon, RangeWeapon
}


public class WeaponItemSO : ItemSO
{
    public int damage;
    public float attackDelay;
    public WeaponType weaponType;

    public WeaponItemSO()
    {
        itemType = ItemType.Weapon;
    }

}
