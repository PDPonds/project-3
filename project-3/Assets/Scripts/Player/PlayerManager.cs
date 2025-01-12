using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Normal, Injury, ShowUI, EndAnyAction, Draging, Action, Aim
}

public class PlayerManager : MonoBehaviour, IDamageable
{
    Vector3 moveDir;
    Rigidbody rb;

    public PlayerDatas playerDatas;

    [HideInInspector] public float curSpeed;

    [Header("===== PlayerState =====")]
    [SerializeField] PlayerState playerState;

    public int maxHP { get; set; }
    public int curHP { get; set; }

    [Header("===== HandSlot =====")]
    public ItemSlot handSlot_1 = new ItemSlot();
    public ItemSlot handSlot_2 = new ItemSlot();

    [Header("==== Attack =====")]
    [HideInInspector] public bool isAttack;
    [SerializeField] Transform bulletSpawnPoint;
    float curAttackDelay;
    bool isAddForceState;
    [HideInInspector] public float reloadTime;

    [Header("===== Drag =====")]
    [HideInInspector] public IDragable curDragObject;

    [Header("===== Action =====")]
    [HideInInspector] public float maxActionTime;
    [HideInInspector] public float actionTime;
    public event Action onAction;

    public void Setup()
    {
        rb = GetComponent<Rigidbody>();
        maxHP = playerDatas.maxHP;
        curHP = playerDatas.curHP;
        SwitchState(PlayerState.Normal);
    }

    private void Update()
    {
        UpdateState();

        DecreaseAttackDelay();

        MoveHandle();
        RotationHandle();

        CheckInteractive();

        Attack();
        Reload();
    }

    #region Movement

    void MoveHandle()
    {
        if (!isAddForceState)
        {
            moveDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
            moveDir = moveDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
            moveDir.Normalize();
            moveDir.y = 0;
            moveDir = moveDir * curSpeed;

            rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
        }
    }

    void RotationHandle()
    {
        if (IsState(PlayerState.Draging) || IsState(PlayerState.Action) || IsState(PlayerState.ShowUI)) return;

        if (!IsState(PlayerState.Aim))
        {
            Vector3 targetDir = Vector3.zero;
            targetDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
            targetDir = targetDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, playerDatas.rotationSpeed * Time.deltaTime);

                transform.rotation = playerRot;
            }
        }
        else
        {
            LookAt(GameManager.Instance.GetWorldPosFormMouse());
        }
    }

    public void TeleportPlayer(Vector3 pos)
    {
        transform.position = pos;
    }

    public void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    IEnumerator AddForce(Vector3 dir, float force, float duration)
    {
        if (!isAddForceState)
        {
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                isAddForceState = true;
                rb.AddForce(dir * force, ForceMode.Impulse);
                yield return null;
            }
        }

        yield return null;
        isAddForceState = false;
    }

    #endregion

    #region Interactive

    public void CheckInteractive()
    {
        if (IsState(PlayerState.Normal) || IsState(PlayerState.Injury))
        {
            Collider[] interactivCol = Physics.OverlapSphere(transform.position, playerDatas.interactiveRange, playerDatas.interactiveMask);
            if (interactivCol.Length > 0)
            {
                Collider targetCol = interactivCol[0];
                if (targetCol.TryGetComponent<IActionObject>(out IActionObject iaction) ||
                    targetCol.TryGetComponent<IDragable>(out IDragable idragable) ||
                    targetCol.TryGetComponent<VehicleObject>(out VehicleObject vehicle))
                {
                    UIManager.Instance.ShowInteractiveKey(targetCol.transform.position);
                    GameManager.Instance.curInteractiveObj = targetCol.gameObject;
                }
                else
                {
                    UIManager.Instance.HideInteractiveKey();
                    GameManager.Instance.curInteractiveObj = null;
                    UIManager.Instance.HideInteractiveChoice();
                }
            }
            else
            {
                UIManager.Instance.HideInteractiveKey();
                GameManager.Instance.curInteractiveObj = null;
                UIManager.Instance.HideInteractiveChoice();
            }
        }
        else if (IsState(PlayerState.ShowUI))
        {
            UIManager.Instance.HideInteractiveKey();
            GameManager.Instance.curInteractiveObj = null;
        }
        else
        {
            UIManager.Instance.HideInteractiveKey();
            GameManager.Instance.curInteractiveObj = null;
            UIManager.Instance.HideInteractiveChoice();
        }
    }

    #endregion

    #region Player State

    public void SwitchToActionState(float duration, Action action)
    {
        onAction = null;
        onAction += action;
        actionTime = duration;
        maxActionTime = duration;
        SwitchState(PlayerState.Action);
        UIManager.Instance.ShowActionDuration();
    }

    public void SwitchState(PlayerState state)
    {
        playerState = state;
        switch (playerState)
        {
            case PlayerState.EndAnyAction:
                SwitchState(PlayerState.Normal);
                break;
        }
    }

    void UpdateState()
    {
        switch (playerState)
        {
            case PlayerState.Normal:

                if (GameManager.Instance.isRunning) curSpeed = playerDatas.runSpeed;
                else curSpeed = playerDatas.walkSpeed;

                break;
            case PlayerState.Injury:

                curSpeed = playerDatas.injurySpeed;

                break;
            case PlayerState.ShowUI:
                curSpeed = 0;
                break;
            case PlayerState.Draging:

                curSpeed = playerDatas.dragingSpeed;

                break;
            case PlayerState.Action:

                curSpeed = 0;

                if (actionTime > 0)
                {
                    actionTime -= Time.deltaTime;
                    if (actionTime <= 0)
                    {
                        onAction?.Invoke();
                        UIManager.Instance.HideActionDuration();
                        SwitchState(PlayerState.EndAnyAction);
                    }
                }

                if (GameManager.Instance.moveInput != Vector2.zero)
                {
                    UIManager.Instance.HideActionDuration();
                    SwitchState(PlayerState.EndAnyAction);
                }

                break;
            case PlayerState.Aim:

                curSpeed = playerDatas.aimSpeed;

                break;
        }
    }

    public bool IsState(PlayerState state)
    {
        return playerState == state;
    }

    #endregion

    #region IDamageable

    public void ResetHP()
    {
        curHP = maxHP;
        playerDatas.curHP = curHP;
    }

    public void TakeDamage(int dmg)
    {
        curHP -= dmg;
        if (curHP <= 0)
        {
            Death();
        }
        playerDatas.curHP = curHP;
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP >= maxHP)
        {
            ResetHP();
        }
        playerDatas.curHP = curHP;
    }

    public void Death()
    {
        Debug.Log("Death");
    }


    #endregion

    #region Hungry And Thirsty
    public void ResetHungry()
    {
        playerDatas.curHungry = playerDatas.maxHungry;
    }

    public void DecreaseHungry(int amount)
    {
        playerDatas.curHungry -= amount;
        if (playerDatas.curHungry <= 0)
        {
            playerDatas.curHungry = 0;
        }
    }

    public void IncreaseHungry(int amount)
    {
        playerDatas.curHungry += amount;
        if (playerDatas.curHungry >= playerDatas.maxHungry)
        {
            ResetHungry();
        }
    }

    public void ResetThirsty()
    {
        playerDatas.curThirsty = playerDatas.maxThirsty;
    }

    public void DecreaseThirsty(int amount)
    {
        playerDatas.curThirsty -= amount;
        if (playerDatas.curThirsty <= 0)
        {
            playerDatas.curThirsty = 0;
        }
    }

    public void IncreaseThirsty(int amount)
    {
        playerDatas.curThirsty += amount;
        if (playerDatas.curThirsty >= playerDatas.maxThirsty)
        {
            ResetThirsty();
        }
    }
    #endregion

    #region Attack
    void DecreaseAttackDelay()
    {
        if (curAttackDelay > 0)
        {
            curAttackDelay -= Time.deltaTime;
            if (curAttackDelay <= 0)
            {
                curAttackDelay = 0;
            }
        }
    }

    void Reload()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab) &&
            itemSlotPrefab.curSlot.item is GunWeaponItemSO gun && itemSlotPrefab.curSlot.curMag <= 0 &&
            GameManager.Instance.playerInventory.HasItem(gun.ammoType, out int ammoIndex))
        {
            if (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
                if (reloadTime <= 0)
                {
                    itemSlotPrefab.curSlot.curMag = gun.maxMagazine;
                    reloadTime = gun.reloadTime;
                }
            }
        }

    }

    public void Attack()
    {
        if (isAttack)
        {
            if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

            if (IsState(PlayerState.Draging))
            {
                curDragObject.EndDrag();
                return;
            }

            if (IsState(PlayerState.ShowUI)) return;

            if (IsState(PlayerState.Action))
            {
                UIManager.Instance.HideActionDuration();
                SwitchState(PlayerState.EndAnyAction);
            }

            if (curAttackDelay <= 0)
            {
                TryAttack();
            }
        }

    }

    void TryAttack()
    {
        if (IsState(PlayerState.Aim))
        {
            if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab))
            {
                if (itemSlotPrefab.curSlot.item is GunWeaponItemSO gun)
                {
                    if (itemSlotPrefab.curSlot.curMag > 0)
                    {
                        GunAttack(gun);
                        itemSlotPrefab.curSlot.curMag--;
                        curAttackDelay = gun.attackDelay;
                    }
                }
                else if (itemSlotPrefab.curSlot.item is ThrowingWeaponItemSO throwing)
                {
                    ThrowingAttack(throwing);
                    curAttackDelay = throwing.attackDelay;
                }
                else
                {
                    SwitchState(PlayerState.EndAnyAction);
                    MeleeAttack();
                    curAttackDelay = itemSlotPrefab.curSlot.item.attackDelay;
                }
            }
            else
            {
                SwitchState(PlayerState.EndAnyAction);
                MeleeAttack();
                curAttackDelay = playerDatas.attackDelay;
            }
        }
        else
        {
            MeleeAttack();
            curAttackDelay = playerDatas.attackDelay;
        }
    }

    void MeleeAttack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, playerDatas.meleeAttackRange, playerDatas.meleeAttackMask);
        Vector3 mouseDir = GameManager.Instance.GetDirToMouse(transform.position);
        Vector3 mousePos = GameManager.Instance.GetWorldPosFormMouse();
        if (cols.Length > 0)
        {
            Collider col = cols[0];
            if (col.TryGetComponent<EnemyManager>(out EnemyManager enemyManager))
            {
                Vector3 dirToEnemy = enemyManager.transform.position - transform.position;
                dirToEnemy.Normalize();
                LookAt(enemyManager.transform.position);
                StartCoroutine(AddForce(dirToEnemy, playerDatas.attackMoveForce, playerDatas.attackMoveDuration));
                //Enemy Take Damage
            }
            else
            {
                LookAt(mousePos);
                StartCoroutine(AddForce(mouseDir, playerDatas.attackMoveForce, playerDatas.attackMoveDuration));
            }
        }
        else
        {
            LookAt(mousePos);
            StartCoroutine(AddForce(mouseDir, playerDatas.attackMoveForce, playerDatas.attackMoveDuration));
        }
        isAttack = false;
    }

    void GunAttack(GunWeaponItemSO gun)
    {
        GameObject bulletObj = Instantiate(gun.bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        BulletObject bullet = bulletObj.GetComponent<BulletObject>();
        bullet.Setup(GameManager.Instance.GetDirToMouse(transform.position), gun.bulletSpeed, gun.bulletTime);

        if (gun.FireType == FireType.Single) isAttack = false;
    }

    void ThrowingAttack(ThrowingWeaponItemSO throwing)
    {

    }

    #endregion

    #region Use Item

    public void UseItem()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;

        if (IsState(PlayerState.Draging))
        {
            curDragObject.EndDrag();
            return;
        }

        if (IsState(PlayerState.Action))
        {
            UIManager.Instance.HideActionDuration();
            SwitchState(PlayerState.EndAnyAction);
        }

        if (IsState(PlayerState.ShowUI))
        {
            return;
        }

        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab))
        {
            if (itemSlotPrefab.curSlot.item is FoodItemSO food)
            {
                SwitchToActionState(food.eatDuration, EatFood);
            }
            else if (itemSlotPrefab.curSlot.item is DrinkItemSO drink)
            {
                SwitchToActionState(drink.drinkDuration, DrinkWater);
            }
            else if (itemSlotPrefab.curSlot.item is HealItemSO healItem)
            {
                SwitchToActionState(healItem.healDuration, UseHeal);
            }
            else if (itemSlotPrefab.curSlot.item is RangeWeaponItemSO rangeWeapon)
            {
                if (IsState(PlayerState.Aim)) SwitchState(PlayerState.EndAnyAction);
                else
                {
                    SwitchState(PlayerState.Aim);
                }
            }
        }

    }

    void EatFood()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab))
        {
            if (itemSlotPrefab.curSlot.item is FoodItemSO food)
            {
                Debug.Log("Eat");
            }
        }
    }

    void DrinkWater()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab))
        {
            if (itemSlotPrefab.curSlot.item is DrinkItemSO drink)
            {
                Debug.Log("Drink");
            }
        }
    }

    void UseHeal()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlotPrefab))
        {
            if (itemSlotPrefab.curSlot.item is HealItemSO healItem)
            {
                Debug.Log("Heal");
            }
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerDatas.interactiveRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDatas.meleeAttackRange);
    }

}
