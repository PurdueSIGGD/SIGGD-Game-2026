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

        public float MoveSpeed => moveSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float MaxVelocity => maxVelocity;
        public float Impulse => impulse;

        [Header("Health")]
        [SerializeField, Tooltip("Health the player starts with.")]
        private float maxHealth = 3f;

        [SerializeField, Tooltip("Health to restore when reviving the player.")]
        private float reviveHealth = 1f;

        [SerializeField, Tooltip("Duration of invincibility frames after taking damage, in seconds.")]
        private float iframeDuration;

        [SerializeField, Tooltip("Speed lost when taking damage, in units per second.")]
        private float damageSpeedDecrease;

        public float MaxHealth => maxHealth;
        public float ReviveHealth => reviveHealth;
        public float IframeDuration => iframeDuration;
        public float DamageSpeedDecrease => damageSpeedDecrease;

        [Header("Ghost Stats")]
        [SerializeField, Tooltip("Base ghost speed before any deaths, in units per second.")]
        private float baseGhostSpeed;

        [SerializeField, Tooltip("Increase in ghost speed per death, in units per second.")]
        private float deathSpeedIncrease;

        public float BaseGhostSpeed => baseGhostSpeed;
        public float DeathSpeedIncrease => deathSpeedIncrease;
    }
}