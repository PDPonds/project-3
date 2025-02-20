using UnityEngine;

public class PlayerDatas : ScriptableObject
{
    [Header("===== Move Speed =====")]
    public float walkSpeed;
    public float runSpeed;
    public float injurySpeed;
    public float dragingSpeed;
    public float aimSpeed;

    [Header("===== Rotation =====")]
    public float rotationSpeed;

    [Header("===== Coin =====")]
    public int coin;

    [Header("===== HP =====")]
    public int maxHP;
    public int curHP;

    [Header("===== Hungry =====")]
    public int maxHungry;
    public int curHungry;

    [Header("===== Thirsty =====")]
    public int maxThirsty;
    public int curThirsty;

    [Header("===== Interactive =====")]
    public float interactiveRange;
    public LayerMask interactiveMask;

    [Header("===== Player Attack =====")]
    public float attackDelay;
    [Header("- Melee Attack")]
    public float meleeDamage;
    public float meleeAttackRange;
    public float mellAttackOffset;
    public LayerMask meleeAttackMask;

    [Header("===== Player Status =====")]
    public Color hpColor;
    public Color hungryColor;
    public Color thirstyColor;
    public Color emptyColor;

    [Header("===== Lock Pick =====")]
    public float lockPickDelay;

}
