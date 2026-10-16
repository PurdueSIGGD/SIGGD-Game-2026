using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject stats;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 currentInputVector;
    private bool brake;
    public float currSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputManager.Instance.OnPlayerMove.AddListener(MovePlayerCharacter);
        
        // Fallback: If not assigned in Inspector, grab the attached Rigidbody2D automatically
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        rb.AddForce(currentInputVector * stats.MoveSpeed, ForceMode2D.Force);
    }

    //The instant brake system
    void Brake()
    {
        currSpeed = moveSpeed; //store the movement speed before the stop
        
        while (brake)
        {
            // remove all speed and stop
            rb.linearVelocity = Vector2.zero;
            moveSpeed -= 0.01f;

            //allow player to redirect their direction

        }

        //launch player in the direction they're holding
    }
}   
