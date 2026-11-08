using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerSystem
{
    /// <summary>
    /// Reads one player's input and exposes it in gameplay terms. Each player has their own PlayerInput, which
    /// pairs them to their own devices, so scripts that read from this only ever hear the player they belong to.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        private const string KeyboardMouseScheme = "Keyboard&Mouse";
        private const float MinAimSqrMagnitude = 0.0001f;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction aimAction;
        private InputAction pointAction;
        private InputAction fireAction;
        private InputAction useToolAction;
        private InputAction useToolAltAction;
        private InputAction brakeAction;
        private Vector2 lastAimDirection = Vector2.right;

        /// <summary>
        /// Movement input, from -1 to 1 on each axis.
        /// </summary>
        public Vector2 Move => moveAction.ReadValue<Vector2>();

        public bool IsBrakeHeld => brakeAction.IsPressed();
        public bool WasBrakePressedThisFrame => brakeAction.WasPressedThisFrame();
        public bool WasBrakeReleasedThisFrame => brakeAction.WasReleasedThisFrame();

        public event Action FirePressed;
        public event Action UseToolPressed;
        public event Action UseToolAltPressed;

        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            // These names match the Player map in Assets/Input/InputActions.inputactions.
            InputActionMap actions = playerInput.actions.FindActionMap("Player", true);
            moveAction = actions.FindAction("Move", true);
            aimAction = actions.FindAction("Aim", true);
            pointAction = actions.FindAction("Point", true);
            fireAction = actions.FindAction("Fire", true);
            useToolAction = actions.FindAction("UseTool", true);
            useToolAltAction = actions.FindAction("UseToolAlt", true);
            brakeAction = actions.FindAction("Brake", true);
        }

        void OnEnable()
        {
            fireAction.performed += OnFirePerformed;
            useToolAction.performed += OnUseToolPerformed;
            useToolAltAction.performed += OnUseToolAltPerformed;
        }

        void OnDisable()
        {
            fireAction.performed -= OnFirePerformed;
            useToolAction.performed -= OnUseToolPerformed;
            useToolAltAction.performed -= OnUseToolAltPerformed;
        }

        /// <summary>
        /// Returns the direction the player is aiming from <paramref name="origin"/>. Mouse players aim at the
        /// cursor and gamepad players aim with the right stick.
        /// </summary>
        public Vector2 GetAimDirection(Vector2 origin)
        {
            Vector2 aim = playerInput.currentControlScheme == KeyboardMouseScheme
                ? GetPointerWorldPosition() - origin
                : aimAction.ReadValue<Vector2>();

            // Keep the last direction while the stick is released or the cursor sits on the origin.
            if (aim.sqrMagnitude > MinAimSqrMagnitude)
            {
                lastAimDirection = aim.normalized;
            }
            return lastAimDirection;
        }

        private Vector2 GetPointerWorldPosition()
        {
            Camera camera = Camera.main;
            Vector2 screenPosition = pointAction.ReadValue<Vector2>();

            // Gameplay happens on the z = 0 plane, so this depth also works for perspective cameras.
            float depth = Mathf.Abs(camera.transform.position.z);
            return camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, depth));
        }

        private void OnFirePerformed(InputAction.CallbackContext context) => FirePressed?.Invoke();

        private void OnUseToolPerformed(InputAction.CallbackContext context) => UseToolPressed?.Invoke();

        private void OnUseToolAltPerformed(InputAction.CallbackContext context) => UseToolAltPressed?.Invoke();
    }
}
