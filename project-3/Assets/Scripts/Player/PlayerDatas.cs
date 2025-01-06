using UnityEngine;

public class PlayerDatas : ScriptableObject
{
    [Header("===== Move Speed =====")]
    public float walkSpeed;
    public float runSpeed;
    public float injurySpeed;
    public float dragingSpeed;

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

}
