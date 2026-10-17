using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject stats;
    private PlayerHealth playerHealth;

    private float ghostSpeed;
    private bool isGhost = false;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerHealth = GetComponent<PlayerHealth>();

        ghostSpeed = 0;
    }

    void FixedUpdate()
    {
        if (isGhost)
        {
            rb.MovePosition(rb.position + new Vector2(0, -1) * ghostSpeed * Time.deltaTime);
            // Handle ghost movement if the player is a ghost
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGhost)
        {
            if (collision.otherCollider.CompareTag("Player")) // temporary tag
            {
                isGhost = false;
                playerHealth.Revive();
            }
            else if (collision.otherCollider.CompareTag("Finish")) // temporary tag
            {
                isGhost = false;
                // player was not revived, no longer in the round
            }
        }
    }


    /// <summary>
    /// Handles the player's death for single-player and multiplayer scenarios.
    /// </summary>
    public void Die()
    {
        // Disable player controls?
        // If single-player, call GameOver()
        // If multiplayer, check if all players are dead. If so, restart current boss. If not, turn into ghost.
        if (ghostSpeed == 0)
        {
            ghostSpeed = stats.BaseGhostSpeed;
        }
        else
        {
            ghostSpeed += stats.DeathSpeedIncrease;
        }
    }

    /// <summary>
    /// Resets the ghost speed and ghost status.
    /// </summary>
    public void Reset()
    {
        ghostSpeed = 0;
        isGhost = false;
    }

    /// <summary>
    /// Handles the single-player game over scenario.
    /// </summary>
    private void GameOver()
    {
        // Game over UI?
        // Restart the current boss
        Reset();
    }

    /// <summary>
    /// Sets the ghost status to true.
    /// </summary>
    private void TurnIntoGhost()
    {
        isGhost = true;
    }

    
}
