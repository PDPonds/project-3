using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Scriptable Objects/Item/Weapon/RangeWeapon/GunWeapon")]

public class GunWeaponItemSO : RangeWeaponItemSO
{
    public float bulletSpeed;
    public float bulletTime;
    public AmmoItemSO ammoType;

    public GunWeaponItemSO()
    {
        rangeWeaponType = RangeWeaponType.GunWeapon;
    }
}
