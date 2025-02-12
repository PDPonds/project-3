using UnityEngine;

public class DoorObject : MonoBehaviour, IActionObject, ILockable
{
    [SerializeField] Transform behideDoorPosition;

    public bool IsLocked { get; set; }

    public void Action()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        GameManager.Instance.curPlayer.TeleportPlayer(behideDoorPosition.position);
    }

    public string ActionName()
    {
        if (IsLocked) return "Unlock";
        else return "Open Door";
    }
}
