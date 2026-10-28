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
    public UnityEvent<Vector2> OnUseMiscItem; 

    public bool IsLeftClickHeld => inputActions != null && inputActions.Player.LeftClick.IsPressed();
    public bool IsRightClickHeld => inputActions != null && inputActions.Player.RightClick.IsPressed();
    public Vector2 PointerScreenPosition => inputActions != null ? inputActions.Player.Look.ReadValue<Vector2>() : Vector2.zero;

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

            // mise use key button (E as of now)
            inputActions.Player.UseMisc.performed += OnMiscUseInput;
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
            inputActions.Player.UseMisc.performed -= OnMiscUseInput;

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

    void OnMiscUseInput(InputAction.CallbackContext context)
    {
        OnUseMiscItem?.Invoke(PointerScreenPosition);
    }
}
