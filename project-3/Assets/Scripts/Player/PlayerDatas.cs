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

    [Header("===== HP =====")]
    public int maxHP;

    [Header("===== Hungry =====")]
    public int maxHungry;

    [Header("===== Thirsty =====")]
    public int maxThirsty;

    [Header("===== Interactive =====")]
    public float interactiveRange;
    public LayerMask interactiveMask;

    [Header("===== Player Attack =====")]
    public float attackDelay;
    [Header("- Melee Attack")]
    public float meleeDamage;
    public float meleeAttackRange;
    public LayerMask meleeAttackMask;
    public float attackMoveForce;
    public float attackMoveDuration;

}
