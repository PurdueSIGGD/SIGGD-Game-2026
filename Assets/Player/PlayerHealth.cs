using UnityEngine;

namespace PlayerSystem
{
    /// <summary>
    /// Manages a player's health and calls the PlayerDeath script when health reaches zero.
    /// </summary>
    [RequireComponent(typeof(PlayerDeath))]
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerData stats;
        public float CurrentHealth { get; private set; }
        public float MaxHealth => stats.MaxHealth;
        public bool IsAlive => CurrentHealth > 0.0f;

        private PlayerDeath playerDeath;
        private PlayerController playerController;
        private float invincibilityTimer = 0f;

        void Awake()
        {
            playerDeath = GetComponent<PlayerDeath>();
            playerController = GetComponent<PlayerController>();
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
            if (!IsAlive || invincibilityTimer > 0f || DebugSettings.Instance.EnablePlayerInvincibility)
            {
                return;
            }

            AudioManager.PlayOneShot(AudioEvents.Sound_Effects.Player.Misc.Player_Hurt);

            CurrentHealth = Mathf.Max(CurrentHealth - context.DamageAmount, 0);
            if (CurrentHealth == 0)
            {
                if (!DebugSettings.Instance.DisablePlayerDeath)
                {
                    playerDeath.Die();
                }
            }
            else
            {
                playerController.PlayerDamaged();
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
        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }
    }
}
