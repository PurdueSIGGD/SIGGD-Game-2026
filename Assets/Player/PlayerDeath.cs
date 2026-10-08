using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private float ghostSpeed;
    [Tooltip("Base ghost speed before any deaths")]
    [SerializeField] private float baseGhostSpeed;
    [Tooltip("Increase in ghost speed per death")]
    [SerializeField] private float deathSpeedIncrease;

    private bool isGhost = false;

    void Awake()
    {
        ghostSpeed = 0;
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
        // Restart the current boss
        Reset();
    }

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
