using UnityEngine;
/// <summary>
/// Manages a player's health and calls the PlayerDeath script when health reaches zero.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float CurrentHealth { get; private set; }
    [field: SerializeField, Tooltip("Maximum health of the player.")]
    public float MaxHealth { get; private set; }
    [Tooltip("The amount of health the player will have after reviving.")]
    [SerializeField] private float reviveHealth;

    [SerializeField] private PlayerDeath playerDeath;
    

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // For testing purposes, press R to revive the player
        {
            Revive();
        }
        if (Input.GetKeyDown(KeyCode.D)) // For testing purposes, press R to revive the player
        {
            DamageContext context = new DamageContext
            {
                DamageAmount = 10f, // Example damage amount
                Attacker = null, // Example attacker (can be set to a GameObject)
                Victim = gameObject // The player is the victim
            };
            TakeDamage(context);
        }
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
    }
}
