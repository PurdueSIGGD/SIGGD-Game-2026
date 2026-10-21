using UnityEngine;
namespace Player
{
    /// <summary>
    /// Manages the player death behavior, including restarting the level or becoming
    /// a ghost.
    /// </summary>
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] private PlayerData stats;
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
        /// Handles the player's death for single-player and multiplayer scenarios.
        /// </summary>
        public void Die()
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0;
            // If single-player, call GameOver()
            if (PlayerRegistery.Players.Length == 1)
            {
                GameOver();
                return;
            }
            // If multiplayer, check if all players are dead. If so, restart current boss. If not, turn into ghost.
            bool allPlayersDead = true;
            for (int i = 0; i < PlayerRegistery.Players.Length; i++)
            {
                if (PlayerRegistery.Players[i] != null && PlayerRegistery.Players[i].Health.IsAlive)
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
                isGhost = true;
                if (ghostSpeed == 0)
                {
                    ghostSpeed = stats.BaseGhostSpeed;
                }
                else
                {
                    ghostSpeed += stats.DeathSpeedIncrease;
                }
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
            // Reset all players (maybe the PlayerRegistery handles this?)
            Reset();
        }

        /// <summary>
        /// Sets the ghost status to true.
        /// </summary>
        private void TurnIntoGhost()
        {
            isGhost = true;
            // Change player appearance?
        }

        private void Revive()
        {
            isGhost = false;
            playerHealth.Revive();
            // Change player appearance back to normal?
        }

        private void RemovePlayer()
        {
            isGhost = false;
            // player was not revived, no longer in the round
            gameObject.SetActive(false); // for now just disable gameobject
        }

    }
}
