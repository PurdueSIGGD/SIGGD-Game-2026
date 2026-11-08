using PlayerSystem;
using UnityEngine;

public class RotateArmToAim : MonoBehaviour
{
    [SerializeField] float angleOffset;

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private PlayerInputHandler input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputHandler>();
    }

    void Update()
    {
        AimDirection = input.GetAimDirection(transform.position);

        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg + angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
