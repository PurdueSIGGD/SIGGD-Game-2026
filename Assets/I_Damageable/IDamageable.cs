using UnityEngine;

public interface IDamageable {

    float CurrentHealth { get; }
    float MaxHealth { get; }
    public void TakeDamage(DamageContext context);
}

public struct DamageContext {
    public float damageAmount;
    public GameObject attacker;
    public GameObject victim;
}
