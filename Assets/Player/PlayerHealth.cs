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

    void Update()
    {
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
<<<<<<< HEAD
    }

    /// <summary>
    /// Revives the player by restoring their health to the revive health value.
    /// </summary>
    public void Revive()
    {
        if (CurrentHealth == 0)
        {
            CurrentHealth = reviveHealth;
        }
    }

    /// <summary>
    /// Resets the player's health status. (For resetting the level)
    /// </summary>
    public void Reset()
    {
        CurrentHealth = MaxHealth;
=======
        
>>>>>>> parent of 74f280c (Add Reset and Revive for player health)
    }
}
