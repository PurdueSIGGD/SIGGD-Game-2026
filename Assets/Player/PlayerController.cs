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

        private InputActions controls;
        private InputAction braking;

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

            if (braking.WasPressedThisFrame())
            {
                Brake();

                //allow player to redirect their direction
                MovePlayerCharacter(currentInputVector);
            }

            if (braking.WasReleasedThisFrame())
            {
                //launch player in the direction they're holding
                rb.AddForce(currentInputVector * currSpeed, ForceMode2D.Impulse);
            }
        }

        //The instant brake system
        void Brake()
        {
            currSpeed = stats.MoveSpeed; //store the movement speed before the stop
            
            // remove all speed and stop
            rb.linearVelocity = Vector2.zero;
            currSpeed -= 0.01f;
        }
    }   
}
