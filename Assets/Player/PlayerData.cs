using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float mass = 1f;
        [SerializeField] private float acceleration = 8f;

        [SerializeField] private float deceleration = 4f;
        [SerializeField] private float maxVelocity = 10f;
        [SerializeField] private float impulse = 20f;
        public float MoveSpeed => moveSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float MaxVelocity => maxVelocity;
        public float Impulse => impulse;
    }
}