namespace Player
{
    using Unity.VisualScripting.Antlr3.Runtime.Misc;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        [SerializeField] private Rigidbody2D rb;
        private Vector2 currentInputVector;
        private float currSpeed;
        private float maxSpeed;

        private InputActions controls;
        private InputAction braking;

        private float storedSpeed = 0f;
        private bool stopHeld = false;
        private bool launchPending = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
            controls = new InputActions();
            braking = controls.Player.Brake;
            
            // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
            rb = GetComponent<Rigidbody2D>();
            controls.Player.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            if (braking.WasPressedThisFrame())
            {
                Brake();
                stopHeld = braking.IsPressed();

                //allow player to redirect their direction
                MovePlayerCharacter(currentInputVector);
            }

            bool dirPressed = currentInputVector != Vector2.zero;
            if (!stopHeld && dirPressed && storedSpeed > 0f && currentInputVector.sqrMagnitude > 0.01f)
            {
                launchPending = true;
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
            //movement
            rb.AddForce(currentInputVector * stats.MoveSpeed, ForceMode2D.Force);

            if (stopHeld)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
            
            if (launchPending)
            {
                rb.linearVelocity = currentInputVector * storedSpeed;
                storedSpeed = 0f;
                launchPending = false;
            }
            if (currentInputVector.sqrMagnitude > 0.01f)
            {
                rb.AddForce(currentInputVector * stats.MoveSpeed, ForceMode2D.Force);
            }
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        //The instant brake system
        void Brake()
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed > 0.1f) storedSpeed = speed;
        }
    }   
}
