using UnityEngine;

public enum PlayerState
{
    Normal, Injury, ShowUI, EndAnyAction, Draging
}

public class PlayerManager : MonoBehaviour, IDamageable
{
    Vector3 moveDir;
    Rigidbody rb;

    [SerializeField] float rotationSpeed;

    [Header("===== Move Speed =====")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float injurySpeed;
    [SerializeField] float dragingSpeed;
    [HideInInspector] public float curSpeed;

    [Header("===== PlayerState =====")]
    [SerializeField] PlayerState playerState;

    public int maxHP { get; set; }
    public int curHP { get; set; }

    [SerializeField] int maxHungry;
    [SerializeField] int curHungry;

    [SerializeField] int maxThirsty;
    [SerializeField] int curThirsty;

    [Header("===== Interactive =====")]
    [SerializeField] float interactiveRange;
    [SerializeField] LayerMask interactiveMask;

    [Header("===== HandSlot =====")]
    public ItemSlot handSlot_1 = new ItemSlot();
    public ItemSlot handSlot_2 = new ItemSlot();

    [Header("===== Drag =====")]
    [HideInInspector] public IDragable curDragObject;

    public void Setup()
    {
        rb = GetComponent<Rigidbody>();
        SwitchState(PlayerState.Normal);
    }

    private void Update()
    {
        UpdateState();

        MoveHandle();
        RotationHandle();

        CheckInteractive();
    }

    #region Movement

    void MoveHandle()
    {
        moveDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
        moveDir = moveDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
        moveDir.Normalize();
        moveDir.y = 0;
        moveDir = moveDir * curSpeed;


        rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);

    }

    void RotationHandle()
    {
        if (IsState(PlayerState.Draging)) return;

        Vector3 targetDir = Vector3.zero;
        targetDir = Camera.main.transform.forward * GameManager.Instance.moveInput.y;
        targetDir = targetDir + Camera.main.transform.right * GameManager.Instance.moveInput.x;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRot;
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

    #endregion

    #region Interactive

    public void CheckInteractive()
    {
        if (IsState(PlayerState.Normal) || IsState(PlayerState.Injury))
        {
            Collider[] interactivCol = Physics.OverlapSphere(transform.position, interactiveRange, interactiveMask);
            if (interactivCol.Length > 0)
            {
                Collider targetCol = interactivCol[0];
                if (targetCol.TryGetComponent<IActionObject>(out IActionObject iaction) ||
                    targetCol.TryGetComponent<IDragable>(out IDragable idragable))
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

    #region Person State

    public void SwitchState(PlayerState state)
    {
        playerState = state;
        switch (playerState)
        {
            case PlayerState.Normal:
                break;
            case PlayerState.Injury:
                break;
            case PlayerState.ShowUI:
                break;
            case PlayerState.EndAnyAction:
                SwitchState(PlayerState.Normal);
                break;
            case PlayerState.Draging:
                break;
        }
    }

    void UpdateState()
    {
        switch (playerState)
        {
            case PlayerState.Normal:

                if (GameManager.Instance.isRunning) curSpeed = runSpeed;
                else curSpeed = walkSpeed;

                break;
            case PlayerState.Injury:

                curSpeed = injurySpeed;

                break;
            case PlayerState.ShowUI:
                curSpeed = 0;
                break;
            case PlayerState.Draging:

                curSpeed = dragingSpeed;

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
    }

    public void TakeDamage(int dmg)
    {
        curHP -= dmg;
        if (curHP <= 0)
        {
            Death();
        }
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP >= maxHP)
        {
            ResetHP();
        }
    }

    public void Death()
    {
        Debug.Log("Death");
    }


    #endregion

    #region Hungry And Thirsty
    public void ResetHungry()
    {
        curHungry = maxHungry;
    }

    public void DecreaseHungry(int amount)
    {
        curHungry -= amount;
        if (curHungry <= 0)
        {
            curHungry = 0;
        }
    }

    public void IncreaseHungry(int amount)
    {
        curHungry += amount;
        if (curHungry >= maxHungry)
        {
            ResetHungry();
        }
    }

    public void ResetThirsty()
    {
        curThirsty = maxThirsty;
    }

    public void DecreaseThirsty(int amount)
    {
        curThirsty -= amount;
        if (curThirsty <= 0)
        {
            curThirsty = 0;
        }
    }

    public void IncreaseThirsty(int amount)
    {
        curThirsty += amount;
        if (curThirsty >= maxThirsty)
        {
            ResetThirsty();
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactiveRange);
    }

}
