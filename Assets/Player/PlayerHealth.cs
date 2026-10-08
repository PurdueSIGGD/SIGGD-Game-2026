using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(DamageContext context)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - context.DamageAmount, 0);
        if (CurrentHealth == 0) {

        }
        
    }
}
