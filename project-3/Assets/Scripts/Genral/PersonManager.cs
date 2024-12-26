using UnityEngine;

public enum PersonState
{
    Normal, Injury, Interact, CancleInteract, Draging
}

public class PersonManager : MonoBehaviour
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


}
