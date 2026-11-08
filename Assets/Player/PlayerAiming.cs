using PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerAiming : MonoBehaviour
{
    private Camera mainCam;
    public bool CanLook = true;

    [SerializeField] private float controllerDeadzone = 0.1f;
    //[SerializeField] private float rotateSmoothing = 1000f;

    [SerializeField] private bool isGamepad;
    private Vector2 gamepadAim;
    void Start()
    {
        mainCam = Camera.main;
        InputManager.Instance.OnPlayerLook.AddListener(RotatePlayerArm);
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
    }

    private void RotatePlayerArm(Vector2 target)
    {
        //mouse 
        Vector2 playerPos = PlayerRegistry.Players[0].transform.position;

        Vector2 direction = (target - playerPos).normalized;
        //Debug.Log(target);
        transform.up = direction;

        //KEEP - Gamepad
        //gamepadAim = Gamepad.current.rightStick.ReadValue();
        //float angle = HandleGamepadAim();
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    float HandleGamepadAim()
    {
        //rotate aim
        if (Mathf.Abs(gamepadAim.x) > controllerDeadzone || Mathf.Abs(gamepadAim.y) > controllerDeadzone)
        {

            if (gamepadAim.sqrMagnitude > 0.0f)
            {
                float angle = Mathf.Atan2(gamepadAim.y, gamepadAim.x);
                if (angle != 0f)
                {
                    angle = angle * Mathf.Rad2Deg + 90f;
                }
                return angle;
            }
        }
        return 0f;
    }

    public void OnControlsChanged (PlayerInput pi)
    {   
        isGamepad = pi.currentControlScheme.Equals("Gamepad");
    }
    
}
