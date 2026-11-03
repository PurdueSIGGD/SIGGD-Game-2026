using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField, Tooltip("Base movement speed in units per second. Not currently used.")]
        private float moveSpeed = 8f;

        [SerializeField, Tooltip("Rigidbody2D mass, used in collisions. Does not affect acceleration or deceleration.")]
        private float mass = 1f;

        [SerializeField, Tooltip("Speed gained while holding a direction, in units per second squared.")]
        private float acceleration = 8f;

        [SerializeField, Tooltip("Speed lost while not holding a direction, in units per second squared.")]
        private float deceleration = 4f;

        [SerializeField, Tooltip("Top speed in units per second.")]
        private float maxVelocity = 10f;

        [SerializeField, Tooltip("Impulse applied when moving from a standstill. Not currently used.")]
        private float impulse = 20f;
        [SerializeField] private float terminalVelocity = 10f;

        public float MoveSpeed => moveSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float TerminalVelocity => terminalVelocity;
        public float Deceleration => deceleration;
        public float MaxVelocity => maxVelocity;
        public float Impulse => impulse;

        [Header("Health")]
        [SerializeField] private float maxHealth = 3f;
        [Tooltip("Health to restore when reviving the player.")]
        [SerializeField] private float reviveHealth = 1f;
        [Tooltip("Duration of invincibility frames after taking damage, in seconds.")]
        [SerializeField] private float iframeDuration;
        [Tooltip("Amount player speed decreases by when taking damage.")]
        [SerializeField] private float damageSpeedDecrease;
        public float MaxHealth => maxHealth;
        public float ReviveHealth => reviveHealth;
        public float IframeDuration => iframeDuration;
        public float DamageSpeedDecrease => damageSpeedDecrease;


        [Header("Ghost Stats")]
        [Tooltip("Base ghost speed before any deaths.")]
        [SerializeField] private float baseGhostSpeed;
        [Tooltip("Increase in ghost speed per death.")]
        [SerializeField] private float deathSpeedIncrease;
        public float BaseGhostSpeed => baseGhostSpeed;
        public float DeathSpeedIncrease => deathSpeedIncrease;
    }
}