using UnityEngine;

public interface IDamageable {

    float CurrentHealth { get; }
    float MaxHealth { get; }
    public void TakeDamage(DamageContext context);
}

public struct DamageContext {
    public float DamageAmount;
    public GameObject Attacker;
    public GameObject Victim;
}