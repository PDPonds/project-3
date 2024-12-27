using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Scriptable Objects/Item/Weapon/RangeWeapon/GunWeapon")]

public class GunWeaponItemSO : RangeWeaponItemSO
{
    public float weaponDurability;
    public float bulletSpeed;
    public float bulletTime;
    public AmmoItemSO ammoType;

    public GunWeaponItemSO()
    {
        itemStackable = false;
        rangeWeaponType = RangeWeaponType.GunWeapon;
    }
}
