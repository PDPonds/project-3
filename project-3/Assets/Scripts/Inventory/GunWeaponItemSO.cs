using UnityEngine;

public enum FireType
{
    Auto, Single
}

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Scriptable Objects/Item/Weapon/RangeWeapon/GunWeapon")]

public class GunWeaponItemSO : RangeWeaponItemSO
{
    public FireType FireType;
    public int maxMagazine;
    public float reloadTime;
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
