using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Scriptable Objects/Item/Weapon/RangeWeapon/ThrowingWeapon")]

public class ThrowingWeaponItemSO : RangeWeaponItemSO
{
    public float attackArea;
    public float attackRange;

    public ThrowingWeaponItemSO()
    {
        rangeWeaponType = RangeWeaponType.ThrowingWeapon;
    }
}
