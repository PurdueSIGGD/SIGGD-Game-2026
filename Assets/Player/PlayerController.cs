using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        private Vector2 currentInputVector;
        private PlayerHealth playerHealth;

        void Awake()
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
        }

        // Update is called once per frame
        void Update()
        {
            // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }
        }

        private void OnDisable()
        {
            // Always unsubscribe from events when destroyed or disabled to prevent memory leaks
            if (InputManager.Instance != null)
            {
                InputManager.Instance.OnPlayerMove.RemoveListener(MovePlayerCharacter);
            }
        }

        void MovePlayerCharacter(Vector2 moveVector)
        {
            currentInputVector = moveVector;
        }

        private void FixedUpdate()
        {
            if (!playerHealth.IsAlive)
            {
                return;
            }
            rb.AddForce(currentInputVector * stats.MoveSpeed, ForceMode2D.Force);
        }
    }
}