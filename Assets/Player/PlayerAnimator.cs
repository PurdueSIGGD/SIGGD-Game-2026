using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (rb.linearVelocityX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rb.linearVelocityX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}

