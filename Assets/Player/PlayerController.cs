using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private InputActionReference moveActionRef;
        private Vector2 currentInputVector;

        private InputActions controls;
        private InputAction braking;

        private float storedSpeed = 0f;
        private bool stopHeld = false;
        private int launchStep = 0;

        // Variables needed initialized in Awake
        void Awake()
        {
            // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
            if (rb == null) rb = GetComponent<Rigidbody2D>();

            rb.linearDamping = 0f; // set to zero currently because of space
            rb.mass = stats.Mass; // from PlayerData.cs

            controls = new InputActions();
            braking = controls.Player.Brake;
        }

        // Update is called once per frame
        void Update()
        {
            currentInputVector = moveActionRef.action.ReadValue<Vector2>();

            if (braking.WasPressedThisFrame())
            {
                Brake();
            }
            if (braking.WasReleasedThisFrame())
            {
                launchStep = 8;
            }
            stopHeld = braking.IsPressed();

            currentInputVector = currentInputVector.normalized;
        }

        private void OnEnable()
        {
            controls.Player.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Disable();
            currentInputVector = Vector2.zero;
        }

        private void OnDestroy()
        {
            controls.Dispose();
        }

        private void FixedUpdate()
        {
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
                rb.AddForce(currentInputVector * stats.Acceleration * rb.mass, ForceMode2D.Force);
            }
            else if (velocity.sqrMagnitude > 0.0001f)
            {
                float maxStopForce = velocity.magnitude * rb.mass / Time.fixedDeltaTime;
                float backForce = Mathf.Min(stats.Deceleration * rb.mass, maxStopForce);
                rb.AddForce(-velocity.normalized * backForce);

            }

            velocity = rb.linearVelocity;
            if (velocity.sqrMagnitude > stats.MaxVelocity * stats.MaxVelocity)
            {
                rb.linearVelocity = velocity.normalized * stats.MaxVelocity;
            }

        }

        // The instant brake system.
        void Brake()
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed > 0.1f) storedSpeed = speed;
        }
    }
}
