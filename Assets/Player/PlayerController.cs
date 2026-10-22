using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        private Vector2 currentInputVector;
        private bool hadLastInput;

        // Variables needed initialized in Awake
        void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();

            rb.linearDamping = 0f; // set to zero currently because of space
            rb.mass = stats.Mass; // from PlayerData.cs
        }

        void Start()
        {
            // void for now
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

        private void OnEnable()
        {
            InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
        }

        private void OnDisable()
        {
            // Always unsubscribe from events when destroyed or disabled to prevent memory leaks
            if (InputManager.Instance != null)
            {
                hadLastInput = false;
                InputManager.Instance.OnPlayerMove.RemoveListener(MovePlayerCharacter);
            }

            currentInputVector = Vector2.zero;
        }

        void MovePlayerCharacter(Vector2 moveVector)
        {
            currentInputVector = Vector2.ClampMagnitude(moveVector, 1f);
        }

        private void FixedUpdate()
        {
            Vector2 velocity = rb.linearVelocity;
            bool hasInput = (currentInputVector.sqrMagnitude > 0.01f);

            if (hasInput)
            {
                if (!hadLastInput && velocity.sqrMagnitude < 0.001f)
                {
                    rb.AddForce(currentInputVector * stats.Impulse, ForceMode2D.Impulse);
                }
                rb.AddForce(currentInputVector * stats.Acceleration, ForceMode2D.Force);
            }
            else if (velocity.sqrMagnitude > 0.0001f)
            {
                float maxStopForce = velocity.magnitude * rb.mass / Time.fixedDeltaTime;
                float backForce = Mathf.Min(stats.Deceleration, maxStopForce);
                rb.AddForce(-velocity.normalized * backForce);

            }

            hadLastInput = hasInput;

            velocity = rb.linearVelocity;
            if (velocity.sqrMagnitude > stats.MaxVelocity * stats.MaxVelocity)
            {
                rb.linearVelocity = velocity.normalized * stats.MaxVelocity;
            }
            
        }
    }
}