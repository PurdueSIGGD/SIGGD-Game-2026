using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 8f;

    private Vector2 currentInputVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    private void OnDisable()
    {
        // Always unsubscribe from events when destroyed or disabled to prevent memory leaks
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnPlayerMove.RemoveListener(MovePlayerCharacter);
        }
    }

    void MovePlayerCharacter(Vector2 moveVector)
    {
        currentInputVector = moveVector;
    }

    private void FixedUpdate()
    {
        rb.AddForce(currentInputVector * moveSpeed, ForceMode2D.Force);
    }
}   
