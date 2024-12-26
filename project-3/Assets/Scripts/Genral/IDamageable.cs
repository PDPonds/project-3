using UnityEngine;

public interface IDamageable
{
    public int maxHP { get; set; }
    public int curHP { get; set; }
    public void ResetHP();
    public void TakeDamage(int dmg);
    public void Heal(int amount);
    public void Death();
}
