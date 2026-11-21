using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

// NOTE: The animation states are auto-synced through NetworkAnimator. HOWEVER, the transitions will not
// sync properly if it is set via a trigger - ummm i dont feel like writing it yet leave a msg for simon
// if you need it, or search for "unity netcode networkanimaor local authority set trigger" :)

/// <summary>
/// Handles the animator params for the player character, such as velocity and facing direction.
/// </summary>
[RequireComponent(typeof(Animator), typeof(NetworkAnimator))]
public class PlayerAnimator : NetworkBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private static readonly int CharVelocityHash = Animator.StringToHash("CharVelocity");

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat(CharVelocityHash, rb.linearVelocity.magnitude);
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

