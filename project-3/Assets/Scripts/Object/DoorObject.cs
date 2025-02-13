using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour, IActionObject, ILockable
{
    [SerializeField] Transform behideDoorPosition;

    public bool IsLocked { get; set; }
    public List<int> LockPos { get; set; }
    [SerializeField] int maxLockCount;
    [SerializeField] int startShowLockCount;
    public int MaxLockCount { get { return maxLockCount; } set { maxLockCount = value; } }
    public int StartShowLockCount { get { return startShowLockCount; } set { startShowLockCount = value; } }

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
            GameManager.Instance.curPlayer.TeleportPlayer(behideDoorPosition.position);
        }
    }

    public void ActionAfterUnlock()
    {
        GameManager.Instance.curPlayer.TeleportPlayer(behideDoorPosition.position);
    }

    public string ActionName()
    {
        if (IsLocked) return "Unlock";
        else return "Open Door";
    }
}
