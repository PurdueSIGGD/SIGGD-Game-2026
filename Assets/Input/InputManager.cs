using Extensions.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] InputActions inputActions;

    public UnityEvent<Vector2> OnPlayerLeftClick;
    public UnityEvent<Vector2> OnPlayerRightClick;
    public UnityEvent<Vector2> OnPlayerMove;

    public bool IsLeftClickHeld => inputActions != null && inputActions.Player.LeftClick.IsPressed();
    public bool IsRightClickHeld => inputActions != null && inputActions.Player.RightClick.IsPressed();
    public Vector2 PointerScreenPosition => Pointer.current != null ? Pointer.current.position.ReadValue() : Vector2.zero;
    public UnityEvent<Vector2> OnPlayerLook;

    protected override void Awake()
    {
        // Executes the base Singleton Awake logic so Instance is assigned properly
        base.Awake();

        // Instantiate InputActions directly in code
        if (inputActions == null)
        {
            inputActions = new InputActions();
        }
    }

    void Update()
    {
        Vector3 playerLookPos = inputActions.Player.Look.ReadValue<Vector2>();
        playerLookPos.z = Camera.main.nearClipPlane;
        playerLookPos = Camera.main.ScreenToWorldPoint(playerLookPos);
        OnPlayerLook?.Invoke(playerLookPos);
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            // enables action map
            inputActions.Player.Enable();

            // subscribes to key pressed and key released
            inputActions.Player.Move.performed += OnPlayerMoveInput;
            inputActions.Player.Move.canceled += OnPlayerMoveInput;

            // subscribes to mouse button presses
            inputActions.Player.LeftClick.performed += OnPlayerLeftClickInput;
            inputActions.Player.RightClick.performed += OnPlayerRightClickInput;
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            // Unsubscribe when disabled to prevent memory leaks
            inputActions.Player.Move.performed -= OnPlayerMoveInput;
            inputActions.Player.Move.canceled -= OnPlayerMoveInput;
            inputActions.Player.LeftClick.performed -= OnPlayerLeftClickInput;
            inputActions.Player.RightClick.performed -= OnPlayerRightClickInput;

            // Disable the map
            inputActions.Player.Disable();
        }
    }
    void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        Vector2 vector = context.ReadValue<Vector2>();
        OnPlayerMove?.Invoke(vector);
    }

    void OnPlayerLeftClickInput(InputAction.CallbackContext context)
    {
        OnPlayerLeftClick?.Invoke(PointerScreenPosition);
    }

    void OnPlayerRightClickInput(InputAction.CallbackContext context)
    {
        OnPlayerRightClick?.Invoke(PointerScreenPosition);
    }
}
