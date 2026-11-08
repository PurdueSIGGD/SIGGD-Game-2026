using PlayerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// run after PlayerAnimator (default order 0) so the body's facing flip is already applied this frame
[DefaultExecutionOrder(100)]
public class PlayerAiming : MonoBehaviour
{
    private Camera mainCam;
    public bool CanLook = true;

    [SerializeField] private float controllerDeadzone = 0.1f;

    [SerializeField] private float spriteAngleOffset = 180f;
    //[SerializeField] private float rotateSmoothing = 1000f;

    [SerializeField] private bool isGamepad;
    private Vector2 gamepadAim;
    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (!CanLook || InputManager.Instance == null) return;
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        Vector3 screenPos = InputManager.Instance.PointerScreenPosition;
        screenPos.z = transform.position.z - mainCam.transform.position.z;
        Vector2 target = mainCam.ScreenToWorldPoint(screenPos);

        RotatePlayerArm(target);
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
        Vector2 direction = target - (Vector2)transform.position;
        if (direction.sqrMagnitude < 0.0001f) return;

        Vector3 localDir = transform.parent.InverseTransformDirection(direction);

        float angle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg - 90f + spriteAngleOffset;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);

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
