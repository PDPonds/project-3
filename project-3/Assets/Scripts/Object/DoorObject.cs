using UnityEngine;

public class DoorObject : MonoBehaviour, IActionObject
{
    [SerializeField] Transform behideDoorPosition;
    [SerializeField] string doorActionName;

    public void Action()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        GameManager.Instance.curPlayer.TeleportPlayer(behideDoorPosition.position);
    }

    public string ActionName()
    {
        return doorActionName;
    }
}
