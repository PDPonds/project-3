using UnityEngine;

public enum PersonState
{
    Normal, Injury, Interact, CancleInteract, Draging
}

public class PersonManager : MonoBehaviour, IDamageable
{
    PlayerManager playerManager;

    [Header("===== Move Speed =====")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float injurySpeed;
    [SerializeField] float dragingSpeed;
    [HideInInspector] public float curSpeed;

    [Header("===== PlayerState =====")]
    PersonState personState;

    public int maxHP { get; set; }
    public int curHP { get; set; }

    [SerializeField] int maxHungry;
    [SerializeField] int curHungry;

    [SerializeField] int maxThirsty;
    [SerializeField] int curThirsty;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        UpdateState();
    }

    #region Person State

    public void SwitchState(PersonState state)
    {
        personState = state;
        switch (personState)
        {
            case PersonState.Normal:
                break;
            case PersonState.Injury:
                break;
            case PersonState.Interact:
                break;
            case PersonState.CancleInteract:
                SwitchState(PersonState.Normal);
                break;
            case PersonState.Draging:
                break;
        }
    }

    void UpdateState()
    {
        switch (personState)
        {
            case PersonState.Normal:

                if (GameManager.Instance.isRunning) curSpeed = runSpeed;
                else curSpeed = walkSpeed;

                break;
            case PersonState.Injury:

                curSpeed = injurySpeed;

                break;
            case PersonState.Interact:
                curSpeed = 0;
                break;
            case PersonState.Draging:

                curSpeed = dragingSpeed;

                break;
        }
    }

    public bool IsState(PersonState state)
    {
        return personState == state;
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

}
