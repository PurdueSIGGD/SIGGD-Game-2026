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
        private GameObject boss;
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
        }

        void FixedUpdate()
        {
            if (isGhost)
            {
                // Handle ghost movement if the player is a ghost...
                if (boss != null)
                {
                    Vector2 direction = (boss.transform.position - transform.position).normalized;
                    rb.MovePosition(rb.position + direction * ghostSpeed * Time.deltaTime);
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (isGhost)
            {
                if (collision.collider.CompareTag(Tags.Player) && collision.collider.GetComponent<PlayerHealth>().IsAlive)
                {
                    Revive();
                }
                else if (collision.collider.CompareTag(Tags.Finish))
                {
                    RemovePlayer();
                }
            }
        }

        /// <summary>
        /// Sets the boss GameObject that the ghost will move toward.
        /// </summary>
        /// <param name="boss">The boss of the level.</param>
        public void SetBoss(GameObject boss)
        {
            this.boss = boss;
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
            for (int i = 0; i < PlayerRegistry.Players.Length; i++)
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
        public void ResetGhost()
        {
            ghostSpeed = 0;
            isGhost = false;
            playerCollider.excludeLayers &= ~layersToIgnore;
        }

        /// <summary>
        /// Handles the game over scenario.
        /// </summary>
        private void GameOver()
        {
            // Game over UI?
            // Restart the current boss
            // Reset all players (maybe the PlayerRegistry handles this?)
            ResetGhost();
        }

        /// <summary>
        /// Sets the ghost status to true.
        /// </summary>
        private void TurnIntoGhost()
        {
            isGhost = true;
            playerCollider.excludeLayers |= layersToIgnore;
            // Change player appearance?
        }

        /// <summary>
        /// Revives the player from ghost state.
        /// </summary>
        private void Revive()
        {
            isGhost = false;
            playerHealth.Revive();
            playerCollider.excludeLayers &= ~layersToIgnore;
            // Change player appearance back to normal?
        }

        /// <summary>
        /// Handles the scenario when the ghost was not revived in time, meaning the player
        /// is out of the round.
        /// </summary>
        private void RemovePlayer()
        {
            isGhost = false;
            playerCollider.excludeLayers &= ~layersToIgnore;
            gameObject.SetActive(false); // for now just disable gameobject
        }

    }
}
