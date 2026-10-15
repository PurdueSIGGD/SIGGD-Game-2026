using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Tooltip("Base ghost speed before any deaths")]
    [SerializeField] private float baseGhostSpeed;
    [Tooltip("Increase in ghost speed per death")]
    [SerializeField] private float deathSpeedIncrease;

    private float ghostSpeed;
    private bool isGhost = false;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    void Awake()
    {
        ghostSpeed = 0;
        //rb = GetComponent<Rigidbody2D>();
        //playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isGhost)
        {
            // Handle ghost movement if the player is a ghost
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
            ghostSpeed = baseGhostSpeed;
        }
        else
        {
            ghostSpeed += deathSpeedIncrease;
        }
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

    /// <summary>
    /// Resets the ghost speed and ghost status.
    /// </summary>
    public void Reset()
    {
        ghostSpeed = 0;
        isGhost = false;
    }
}
