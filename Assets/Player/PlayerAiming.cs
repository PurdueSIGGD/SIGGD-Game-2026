using PlayerSystem;
using UnityEngine;

/**
 * <summary>
 * Rotates a character's arm sprite toward where the player is aiming. The aim comes from the player's
 * PlayerInputHandler, so mouse players aim at the cursor and gamepad players aim with the right stick.
 * </summary>
 */
// run after PlayerAnimator (default order 0) so the body's facing flip is already applied this frame
[DefaultExecutionOrder(100)]
public class PlayerAiming : MonoBehaviour
{
    public bool CanLook = true;

    [SerializeField] private float spriteAngleOffset = 180f;
    //[SerializeField] private float rotateSmoothing = 1000f;

    private PlayerInputHandler input;

    void Awake()
    {
        input = GetComponentInParent<PlayerInputHandler>();
    }

    void LateUpdate()
    {
        if (!CanLook) return;

        RotatePlayerArm(input.GetAimDirection(transform.position));
    }

    private void RotatePlayerArm(Vector2 direction)
    {
        // Rotate in the parent's space so the arm stays correct while the body is flipped.
        Vector3 localDir = transform.parent.InverseTransformDirection(direction);

        float angle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg - 90f + spriteAngleOffset;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
