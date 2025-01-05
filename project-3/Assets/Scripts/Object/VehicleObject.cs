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
    }

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

    #endregion

    public UnityAction onDrive;
    public UnityAction onFillGas;
    public UnityAction onRepair;

    private void Start()
    {
        onDrive += Drive;
        onFillGas += FillGas;
        onRepair += Repair;
    }

    void Drive()
    {
        Debug.Log("Drive");
    }

    void FillGas()
    {
        Debug.Log("Fill Gas");
    }

    void Repair()
    {
        Debug.Log("Repair");
    }

    #region Gas

    public void DecreaseGas(int amount)
    {
        curGas -= amount;
        if (curGas <= 0)
        {
            curGas = 0;
        }
    }

    public void IncreaseGas(int amount)
    {
        curGas += amount;
        if (curGas >= maxGas)
        {
            curGas = maxGas;
        }
    }


    public bool EnoughGas(int amount)
    {
        return curGas >= amount;
    }

    #endregion

}
