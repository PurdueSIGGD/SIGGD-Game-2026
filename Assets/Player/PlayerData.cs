using UnityEngine;

namespace PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerData : ScriptableObject
    {
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
    }
}