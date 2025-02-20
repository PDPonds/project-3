using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamageable
{
    public int maxHP { get; set; }
    public int curHP { get; set; }

    #region IDamageable

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP > maxHP)
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
        Debug.Log("T");
        curHP -= dmg;
        if (curHP <= 0)
        {
            curHP = 0;
        }
    }
    #endregion

}
