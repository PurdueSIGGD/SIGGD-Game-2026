using UnityEngine;
/// <summary>
/// Interface for damageable entities. Any entity that can take damage should implement this interface.
/// </summary>
public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsAlive => CurrentHealth > 0;
    public void TakeDamage(DamageContext context);
}

/// <summary>
/// Struct for containing damage information.
/// </summary>
public struct DamageContext
{
    public float DamageAmount;
    public GameObject Attacker;
    public GameObject Victim;
}