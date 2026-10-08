using UnityEngine;
/// <summary>
/// Manages a player's health and calls the PlayerDeath script when health reaches zero.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; private set; }
    [field: SerializeField, Tooltip("Maximum health of the player.")]
    public float MaxHealth { get; private set; }

    [SerializeField] private PlayerDeath playerDeath;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    /// <summary>
    /// Applies damage to the player and handles death.
    /// </summary>
    /// <param name="context">The damage context containing damage information.</param>
    public void TakeDamage(DamageContext context)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - context.DamageAmount, 0);
        if (CurrentHealth == 0) {
            playerDeath.Die();
        }
        
    }
}
