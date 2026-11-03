namespace Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private InputActionReference moveActionRef;
        private Vector2 currentInputVector;

        private float x, y;

        private InputActions controls;
        private InputAction braking;
        private InputAction movement;

        private float storedSpeed = 0f;
        private bool stopHeld = false;
        private int launchStep = 0;
        private bool hadLastInput;

        private float brakeTimer;

        // Variables needed initialized in Awake
        void Awake()
        {
            // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
            if (rb == null) rb = GetComponent<Rigidbody2D>();

            rb.linearDamping = 0f; // set to zero currently because of space
            rb.mass = stats.Mass; // from PlayerData.cs
        }

        void Start()
        {
            controls = new InputActions();
            braking = controls.Player.Brake;

            controls.Player.Enable();
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

            //sets the x and y values of the input vector
            float x = 0f, y = 0f;
            if (currentInputVector.x < 0)
                x -= 1f;
            if (currentInputVector.x > 0)
                x += 1f;
            if (currentInputVector.y > 0)
                y += 1f;
            if (currentInputVector.y < 0)
                y -= 1f;
            currentInputVector = currentInputVector.normalized;
        }

        private void OnDisable()
        {
            currentInputVector = Vector2.zero;
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
                // if (!hadLastInput && velocity.sqrMagnitude < 0.001f)
                // {
                //     rb.AddForce(currentInputVector * stats.Impulse, ForceMode2D.Impulse);
                // }
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

        //The instant brake system
        void Brake()
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed > 0.1f) storedSpeed = speed;
        }
    }
}
