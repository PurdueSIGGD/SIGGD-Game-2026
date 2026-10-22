using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float mass = 2f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float terminalVelocity = 10f;
        public float MoveSpeed => moveSpeed;
        public float Mass => mass;
        public float Acceleration => acceleration;
        public float TerminalVelocity => terminalVelocity;
    }
}