using UnityEngine;
using UnityEngine.InputSystem;

/*
 * this whole script will probably be temporary as once we have actual art and guns n shit itll need to be done differently but it works for now
 */

public class RotateArmToMouse : MonoBehaviour
{
    [SerializeField] float angleOffset;
    void Update()
    {
        // Safety check to prevent errors if the mouse isn't active
        if (Mouse.current == null) return;

        // Get mouse position on the screen
        Vector2 mouseScreenPosition = InputManager.Instance.PointerScreenPosition;

        // Fix: Calculate the correct Z distance from the camera to the player/arm
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToCamera));

        // Calculate direction vector from arm to mouse
        Vector2 direction = mouseWorldPosition - transform.position;

        // Calculate angle and apply offset
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
