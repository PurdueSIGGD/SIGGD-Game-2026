using UnityEngine;
namespace PlayerSystem
{
    /// <summary>
    /// Manages the player death behavior, including restarting the level or becoming
    /// a ghost.
    /// </summary>
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
        private PlayerHealth playerHealth;
        private PlayerController playerController;

        private float ghostSpeed;
        private bool isGhost = false;
        private Vector2 ghostMoveDirection;
        [Tooltip("Layers to ignore while the player is a ghost.")]
        [SerializeField] private LayerMask layersToIgnore;

        private Rigidbody2D rb;
        private Collider2D playerCollider;


        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<Collider2D>();
            playerHealth = GetComponent<PlayerHealth>();
            playerController = GetComponent<PlayerController>();

            ghostSpeed = 0;
            SetGhostMoveDirection(Vector2.down); // Default direction, can be changed later
        }

        void FixedUpdate()
        {
            if (isGhost)
            {
                rb.MovePosition(rb.position + ghostMoveDirection * ghostSpeed * Time.deltaTime);
                // Handle ghost movement if the player is a ghost...
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (isGhost)
            {
                if (collision.collider.CompareTag("Player")) // temporary tag
                {
                    Revive();
                }
                else if (collision.collider.CompareTag("Finish")) // temporary tag
                {
                    RemovePlayer();
                }
            }
        }

        /// <summary>
        /// Sets the direction that the ghost should move in, in case
        /// of different planets being placed on different sides of the screen.
        /// </summary>
        /// <param name="direction">Direction for the ghost to move in.</param>
        public void SetGhostMoveDirection(Vector2 direction)
        {
            ghostMoveDirection = direction.normalized;
        }

        /// <summary>
        /// Handles the player's death for single-player and multiplayer scenarios.
        /// </summary>
        public void Die()
        {
            playerController.PlayerDeath();
            // Single-player behavior
            if (!PlayerRegistry.IsMultiplayer)
            {
                GameOver();
                return;
            }
            // Multiplayer behavior
            bool allPlayersDead = true;
            for (int i = 0; i < PlayerRegistry.PlayerCount; i++)
            {
                if (PlayerRegistry.Players[i] != null && PlayerRegistry.Players[i].Health.IsAlive)
                {
                    allPlayersDead = false;
                    break;
                }
            }
            if (allPlayersDead)
            {
                GameOver();
            }
            else
            {
                if (ghostSpeed == 0)
                {
                    ghostSpeed = stats.BaseGhostSpeed;
                }
                else
                {
                    ghostSpeed += stats.DeathSpeedIncrease;
                }
                TurnIntoGhost();
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
        /// Handles the game over scenario.
        /// </summary>
        private void GameOver()
        {
            // Game over UI?
            // Restart the current boss
            // Reset all players (maybe the PlayerRegistry handles this?)
            Reset();
        }

        /// <summary>
        /// Sets the ghost status to true.
        /// </summary>
        private void TurnIntoGhost()
        {
            isGhost = true;
            playerCollider.excludeLayers += layersToIgnore;
            // Change player appearance?
        }

        /// <summary>
        /// Revives the player from ghost state.
        /// </summary>
        private void Revive()
        {
            isGhost = false;
            playerHealth.Revive();
            playerCollider.excludeLayers -= layersToIgnore;
            // Change player appearance back to normal?
        }

        /// <summary>
        /// Handles the scenario when the ghost was not revived in time, meaning the player
        /// is out of the round.
        /// </summary>
        private void RemovePlayer()
        {
            isGhost = false;
            gameObject.SetActive(false); // for now just disable gameobject
        }

    }
}
