using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager player) ||
            other.TryGetComponent<VehicleObject>(out VehicleObject vehicle))
        {
            ExitMap();
        }
    }

    void ExitMap()
    {
        GameManager.Instance.SwitchPhase(GamePhase.EndGame);
    }

}
