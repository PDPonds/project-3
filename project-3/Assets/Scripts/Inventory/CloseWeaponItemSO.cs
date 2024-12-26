using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSO", menuName = "Scriptable Objects/Item/Weapon/CloseWeapon")]
public class CloseWeaponItemSO : WeaponItemSO
{
    public float attackRange;

    public CloseWeaponItemSO()
    {
        weaponType = WeaponType.CloseWeapon;
    }
}
