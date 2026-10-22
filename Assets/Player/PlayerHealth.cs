using UnityEngine;
namespace Player
{
    /// <summary>
    /// Manages a player's health and calls the PlayerDeath script when health reaches zero.
    /// </summary>
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerData stats;
        public float CurrentHealth { get; private set; }
        public float MaxHealth => stats.MaxHealth;
        public bool IsAlive => CurrentHealth > 0;

        private PlayerDeath playerDeath;
        private float invincibilityTimer = 0f;

        void Awake()
        {
            playerDeath = GetComponent<PlayerDeath>();
            CurrentHealth = MaxHealth;
        }

        void Update()
        {
            if (invincibilityTimer > 0f)
            {
                invincibilityTimer = Mathf.Max(invincibilityTimer - Time.deltaTime, 0f);
            }
        }

        /// <summary>
        /// Applies damage to the player and handles death.
        /// </summary>
        /// <param name="context">The damage context containing damage information.</param>
        public void TakeDamage(DamageContext context)
        {
            if (invincibilityTimer > 0f)
            {
                Debug.Log("iframe active, no damage taken");
                return;
            }
            CurrentHealth = Mathf.Max(CurrentHealth - context.DamageAmount, 0);
            Debug.Log("Player took damage: " + context.DamageAmount + ", remaining health: " + CurrentHealth);
            if (CurrentHealth == 0)
            {
                Debug.Log("Player died");
                playerDeath.Die();
            }
            invincibilityTimer = stats.IframeDuration;
        }

        /// <summary>
        /// Revives the player by restoring their health to the revive health value.
        /// </summary>
        public void Revive()
        {
            if (CurrentHealth == 0)
            {
                CurrentHealth = stats.ReviveHealth;
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
}
