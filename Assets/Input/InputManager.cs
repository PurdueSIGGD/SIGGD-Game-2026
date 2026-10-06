using Extensions.Singleton;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] InputActions inputActions;

    public UnityEvent<Vector2> OnPlayerMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions.Player.Move.performed += OnPlayerMoveInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerMoveInput(InputAction.CallbackContext context)
    {
        var vector = context.ReadValue<Vector2>();
        OnPlayerMove?.Invoke(vector);
    }
}
