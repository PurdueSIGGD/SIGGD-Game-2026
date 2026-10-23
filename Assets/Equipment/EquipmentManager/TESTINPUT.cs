using UnityEngine;
using UnityEngine.InputSystem;

public class TESTINPUT : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current == null)
            return;

        // Same as Input.GetMouseButtonDown(0)
        if (Mouse.current.leftButton.wasPressedThisFrame)
            EquipmentManager.Instance.shootPrimaryWeapon();

        // Same as Input.GetMouseButton(1)
        if (Mouse.current.rightButton.isPressed)
            EquipmentManager.Instance.shootMiscellaneous();
    }
}