using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(PlayerHealth), typeof(PlayerInputHandler))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        private PlayerHealth playerHealth;
        private PlayerInputHandler input;
        private Vector2 currentInputVector;

        private float storedSpeed = 0f;
        private bool stopHeld = false;
        private int launchStep = 0;
        private bool hadLastInput = false;

        // Variables needed initialized in Awake
        void Awake()
        {
            // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            playerHealth = GetComponent<PlayerHealth>();
            input = GetComponent<PlayerInputHandler>();

            rb.linearDamping = 0f; // set to zero currently because of space
            rb.mass = stats.Mass; // from PlayerData.cs
        }

        // Update is called once per frame
        void Update()
        {
            if (!playerHealth.IsAlive)
            {
                return;
            }
            currentInputVector = input.Move;

            if (input.WasBrakePressedThisFrame)
            {
                Brake();
            }
            if (input.WasBrakeReleasedThisFrame)
            {
                launchStep = 8;
            }
            stopHeld = input.IsBrakeHeld;

            currentInputVector = currentInputVector.normalized;
        }

        private void OnDisable()
        {
            hadLastInput = false;
            currentInputVector = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (!playerHealth.IsAlive)
            {
                return;
            }

            if (stopHeld)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (launchStep > 0)
            {
                if (currentInputVector.sqrMagnitude > 0.01f)
                {
                    rb.linearVelocity = currentInputVector * storedSpeed;
                    storedSpeed = 0f;
                    launchStep = 0;
                }
                launchStep--;
            }

            Vector2 velocity = rb.linearVelocity;
            bool hasInput = currentInputVector.sqrMagnitude > 0.01f;

            if (hasInput)
            {
                if (!hadLastInput && velocity.sqrMagnitude < 0.001f)
                {
                    rb.AddForce(currentInputVector * stats.Impulse, ForceMode2D.Impulse);
                }
                rb.AddForce(currentInputVector * stats.Acceleration, ForceMode2D.Force);            }
            else if (velocity.sqrMagnitude > 0.0001f)
            {
                float maxStopForce = velocity.magnitude * rb.mass / Time.fixedDeltaTime;
                float backForce = Mathf.Min(stats.Deceleration * rb.mass, maxStopForce);
                rb.AddForce(-velocity.normalized * backForce);

            }
            hadLastInput = hasInput;

            velocity = rb.linearVelocity;
            if (velocity.sqrMagnitude > stats.MaxVelocity * stats.MaxVelocity)
            {
                rb.linearVelocity = velocity.normalized * stats.MaxVelocity;
            }

        }

        /// <summary>
        /// Handles the player being damaged, reducing their speed.
        /// </summary>
        public void PlayerDamaged()
        {
            if (stopHeld || launchStep > 0)
            {
                storedSpeed = Mathf.Max(storedSpeed - stats.DamageSpeedDecrease, 0f);
            }
            else
            {
                if (stats.DamageSpeedDecrease * stats.DamageSpeedDecrease >= rb.linearVelocity.sqrMagnitude)
                {
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    rb.linearVelocity -= rb.linearVelocity.normalized * stats.DamageSpeedDecrease;
                }
            }
            // Implement iframe animations?
            // Apply displacement manually?
        }

        /// <summary>
        /// Stops all inputs and movement on player death.
        /// </summary>
        public void PlayerDeath()
        {
            rb.linearVelocity = Vector2.zero;
            storedSpeed = 0f;
            stopHeld = false;
            currentInputVector = Vector2.zero;
        }

        // The instant brake system.
        void Brake()
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed > 0.1f) storedSpeed = speed;
        }
    }
}
