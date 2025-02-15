using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour, IActionObject, ILockable
{
    [Header("===== Font Door Position =====")]
    public Transform fontDoorPosition;

    Transform behideDoorPosition;

    [Header("===== Cam Rotation =====")]
    [SerializeField] float YRotation;
    [SerializeField] float moveRotationSpeed;

    public bool IsLocked { get; set; }
    public List<int> LockPos { get; set; }
    [Header("===== Lock =====")]
    [SerializeField] int maxLockCount;
    [SerializeField] int startShowLockCount;
    public int MaxLockCount { get { return maxLockCount; } set { maxLockCount = value; } }
    public int StartShowLockCount { get { return startShowLockCount; } set { startShowLockCount = value; } }

    [HideInInspector] public bool isBuildingTile;
    [HideInInspector] public GameObject buildingObj;

    private void Start()
    {
        IsLocked = true;
        Setup();
    }

    void Setup()
    {
        if (IsLocked)
        {
            LockPos = new List<int>();
            for (int i = 1; i <= MaxLockCount; i++)
            {
                LockPos.Add(i);
            }
            LockPos = GameManager.Instance.ShuffleInt(LockPos);
        }
    }

    public void SetBehideDoorPosition(Transform b)
    {
        behideDoorPosition = b;
    }

    public void Action()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        if (IsLocked)
        {
            UIManager.Instance.ShowLockPick(LockPos, StartShowLockCount, this);
        }
        else
        {
            ActionAfterUnlock();
        }
    }

    public void ActionAfterUnlock()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        GameManager.Instance.curPlayer.TeleportPlayer(behideDoorPosition.position);
        GameManager.Instance.curCameraController.SetYRotation(YRotation, moveRotationSpeed);
        if (isBuildingTile) buildingObj.SetActive(true);
        else buildingObj.SetActive(false);
        UIManager.Instance.CloseInteractiveChoice();
        GameManager.Instance.curPlayer.SwitchState(PlayerState.EndAnyAction);

    }

    public string ActionName()
    {
        if (IsLocked) return "Unlock";
        else return "Open Door";
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (fontDoorPosition != null)
        {
            Gizmos.DrawWireCube(fontDoorPosition.position, Vector3.one * 0.1f);
        }
    }


}
