using UnityEngine;

public enum RangeWeaponType
{
    GunWeapon, ThrowingWeapon
}

public class RangeWeaponItemSO : WeaponItemSO
{
    public GameObject bulletPrefab;
    public RangeWeaponType rangeWeaponType;

    public RangeWeaponItemSO()
    {
        weaponType = WeaponType.RangeWeapon;
    }
}
