using Extensions.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] InputActions inputActions;

    public UnityEvent<Vector2> OnPlayerMove;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions.Player.Move.performed += OnPlayerMoveInput;
    }

    // Update is called once per frame
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
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            // Unsubscribe when disabled to prevent memory leaks
            inputActions.Player.Move.performed -= OnPlayerMoveInput;
            inputActions.Player.Move.canceled -= OnPlayerMoveInput;

            // Disable the map
            inputActions.Player.Disable();
        }
    }
    void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        Vector2 vector = context.ReadValue<Vector2>();
        OnPlayerMove?.Invoke(vector);
    }
}
