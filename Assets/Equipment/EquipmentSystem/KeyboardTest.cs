using UnityEngine;
using UnityEngine.InputSystem;

// Requires a PlayerInput component on this same GameObject, set up as:
//   Actions          -> Assets/Input/InputActions.inputactions
//   Default Map      -> Player
//   Behavior         -> Send Messages
// With that set up, pressing 0 / 1 / 2 fires the "Equip0" / "Equip1" / "Equip2"
// actions (bound to those keys in InputActions.inputactions), which the
// Input System turns into calls to the matching On<ActionName> method below -
// the same effect as the old Input.GetKeyDown(KeyCode.Alpha0/1/2) checks.
[RequireComponent(typeof(PlayerInput))]
public class KeyboardTest : MonoBehaviour
{
    public PlayerEquipmentSystem playerEquipmentSystem;

    // Called by PlayerInput when the "Equip0" action fires (key "0").
    public void OnEquip3(InputValue value)
    {
        if (value.isPressed)
            playerEquipmentSystem.setEquipment(2);
    }

    // Called by PlayerInput when the "Equip1" action fires (key "1").
    public void OnEquip1(InputValue value)
    {
        if (value.isPressed)
            playerEquipmentSystem.setEquipment(0);
    }

    // Called by PlayerInput when the "Equip2" action fires (key "2").
    public void OnEquip2(InputValue value)
    {
        if (value.isPressed)
            playerEquipmentSystem.setEquipment(1);
    }
}
