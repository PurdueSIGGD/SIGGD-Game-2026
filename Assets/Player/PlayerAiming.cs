using PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : MonoBehaviour
{
    private Camera mainCam;
    public bool CanLook = true;

    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private float rotateSmoothing = 1000f;

    [SerializeField] private bool isGamepad;
    private Vector2 gamepadAim;
    private PlayerInput input;
    private InputAction lookAction;

    void Awake()
    {
        InputActionMap inputMap = input.actions.FindActionMap("InputActions");
        lookAction = inputMap.FindAction("Look");   
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
        InputManager.Instance.OnPlayerLook.AddListener(RotatePlayerArm);
        
        isGamepad = true;
    }

    // Update is called once per frame
    void Update()
    {
        //mousePos = ;
        //mousePos.z = 0f;
        //Vector3 mousePos = Mouse.current.position.ReadValue();
        //mousePos.z = Camera.main.nearClipPlane;
        //worldPos = Camera.main.ScreenToWorldPoint(mousePos);


        //Debug.Log(Mouse.current.position.ReadValue());

        //Vector3 direction = worldPos - transform.position;
        ////print(direction);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log(angle);

        //transform.rotation = Quaternion.Euler(0, 0, angle);

        if(isGamepad)
        {
            HandleGamepadInput();
            HandleGamepadAim();
        }
    }

    private void RotatePlayerArm(Vector2 target)
    {
        Debug.Log(target);

        Vector2 playerPos = PlayerRegistry.Players[0].transform.position;

        Vector2 direction = target - playerPos;
        //print(direction);
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ////Debug.Log(angle);

        //transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.up = -direction;
    }

    //gamepad functions
    void HandleGamepadInput()
    {
        //gamepadAim = Gamepad.current.rightStick.ReadValue();
        gamepadAim = lookAction.ReadValue<Vector2>();
    }
    
    void HandleGamepadAim()
    {
        //rotate aim
        if (Mathf.Abs(gamepadAim.x) > controllerDeadzone || Mathf.Abs(gamepadAim.y) > controllerDeadzone)
        {
            Vector3 playerDirection = Vector3.right * gamepadAim.x + Vector3.forward * gamepadAim.y;

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotateSmoothing * Time.deltaTime);
            }
        }
    }

    public void OnControlsChanged (PlayerInput pi)
    {   
        isGamepad = pi.currentControlScheme.Equals("Gamepad");
    }
    
}
