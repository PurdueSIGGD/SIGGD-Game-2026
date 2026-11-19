using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    /// <summary>
    /// Handles player spawning for each scene based on information from the shop.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        [Tooltip("List of positions where players can spawn")]
        [SerializeField] private Vector2[] spawnPositions;
        [SerializeField] private GameObject boss;
        private List<int> availablePositions;

        void Start()
        {
            availablePositions = new List<int>();
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                availablePositions.Add(i);
            }

            // TESTING
            SpawnPlayer(Character.Scope);
            SpawnPlayer(Character.Scope);
        }

        /// <summary>
        /// Spawns players into the scene with the given character and inventory information.
        /// </summary>
        /// <param name="character">The character to spawn.</param>
        public void SpawnPlayer(Character character) // Need a way to pass player's inventory
        {
            if (availablePositions.Count == 0)
            {
                Debug.LogError("No available spawn positions for new player.");
                return;
            }
            // Choose a random available spawn position
            int randomIndex = Random.Range(0, availablePositions.Count);
            Player newPlayer = Instantiate(CharacterPrefabRegistry.Prefabs[character], spawnPositions[availablePositions[randomIndex]], Quaternion.identity);
            availablePositions.RemoveAt(randomIndex);
            // Initialize player information
            newPlayer.Death.SetBoss(boss);
            // Inventory info?
        }
    }
}