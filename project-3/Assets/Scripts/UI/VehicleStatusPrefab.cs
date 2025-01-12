using TMPro;
using UnityEngine;

public class VehicleStatusPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gasCount;
    [SerializeField] TextMeshProUGUI hpCount;

    public void UpdateStatus(int maxGas, int curGas, int maxHp, int curHP)
    {
        gasCount.text = $"{curGas} / {maxGas}";
        hpCount.text = $"{curHP} / {maxHp}";
    }

}
