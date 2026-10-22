using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float mass = 2f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float terminalVelocity = 10f;
        public float MoveSpeed => moveSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float TerminalVelocity => terminalVelocity;

        [Header("Health")]
        [SerializeField] private float maxHealth = 3f;
        [Tooltip("Health to restore when reviving the player.")]
        [SerializeField] private float reviveHealth = 1f;
        [Tooltip("Duration of invincibility frames after taking damage, in seconds.")]
        [SerializeField] private float iframeDuration;
        public float MaxHealth => maxHealth;
        public float ReviveHealth => reviveHealth;
        public float IframeDuration => iframeDuration;

        [Header("Ghost Stats")]
        [Tooltip("Base ghost speed before any deaths.")]
        [SerializeField] private float baseGhostSpeed;
        [Tooltip("Increase in ghost speed per death.")]
        [SerializeField] private float deathSpeedIncrease;
        public float BaseGhostSpeed => baseGhostSpeed;
        public float DeathSpeedIncrease => deathSpeedIncrease;
    }
}