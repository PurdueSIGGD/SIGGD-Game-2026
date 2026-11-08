using UnityEngine;
using UnityEngine.InputSystem;

public class RotateArmToMouse : MonoBehaviour
{
    [SerializeField] float angleOffset;

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        float zDistance = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, zDistance));

        Vector2 direction = (Vector2)mouseWorldPosition - (Vector2)transform.position;
        if (direction.sqrMagnitude > 0.0001f) AimDirection = direction.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
