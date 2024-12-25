using UnityEngine;

public enum PlayerState
{
    Normal, Injury, Draging
}

public class PersonManager : MonoBehaviour
{
    [Header("===== Move Speed =====")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float injurySpeed;
    [SerializeField] float dragingSpeed;
    [HideInInspector] public float curSpeed;

    [Header("===== PlayerState =====")]
    PlayerState playerState;

    private void Update()
    {
        UpdateState();
    }

    #region Player State

    public void SwitchState(PlayerState state)
    {
        playerState = state;
        switch (playerState)
        {
            case PlayerState.Normal:
                break;
            case PlayerState.Injury:
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

}
