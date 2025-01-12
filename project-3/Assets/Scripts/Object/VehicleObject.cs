using System;
using UnityEngine;
using UnityEngine.Events;

public class VehicleObject : MonoBehaviour, IDamageable
{
    public int maxHP { get; set; }
    public int curHP { get; set; }

    [HideInInspector] public int curGas;
    [HideInInspector] public int maxGas;

    #region IDamageable

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);

    }

    public void ResetHP()
    {
        curHP = maxHP;
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);

    }

    public void TakeDamage(int dmg)
    {
        curHP -= dmg;
        if (curHP <= 0)
        {
            Death();
        }
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);

    }

    #endregion

    public UnityAction onDrive;
    public UnityAction onFillGas;
    public UnityAction onRepair;

    [HideInInspector] public VehicleStatusPrefab curStatusInfo;

    private void Start()
    {
        onDrive += Drive;
        onFillGas += StartFillGas;
        onRepair += StartRepair;
    }

    void Drive()
    {
        GameManager.Instance.SwitchPhase(GamePhase.EndGame);
    }

    void StartFillGas()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot))
        {
            if (itemSlot.curSlot.item is GasTankItemSO gasTank &&
            itemSlot.curSlot.curValue > 0)
            {
                GameManager.Instance.curPlayer.SwitchToActionState(gasTank.fillDuration, FillGas);
            }
        }
    }

    void FillGas()
    {
        Debug.Log("Fill Gas");
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);
    }

    void StartRepair()
    {
        if (GameManager.Instance.curHandSlot.HasItemInSlot(out ItemSlotPrefab itemSlot))
        {
            if (itemSlot.curSlot.item is GearBoxItemSO gearBox &&
            itemSlot.curSlot.curValue > 0)
            {
                GameManager.Instance.curPlayer.SwitchToActionState(gearBox.repairDuration, Repair);

            }
        }
    }

    void Repair()
    {
        Debug.Log("Repair");
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);
    }

    #region Gas

    public void DecreaseGas(int amount)
    {
        curGas -= amount;
        if (curGas <= 0)
        {
            curGas = 0;
        }
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);
    }

    public void IncreaseGas(int amount)
    {
        curGas += amount;
        if (curGas >= maxGas)
        {
            curGas = maxGas;
        }
        curStatusInfo?.UpdateStatus(maxGas, curGas, maxHP, curHP);
    }


    public bool EnoughGas(int amount)
    {
        return curGas >= amount;
    }

    #endregion

}
