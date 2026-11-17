using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    /// <summary>
    /// Handles player spawning based on information from the shop.
    /// </summary>
    public class PlayerSpawner : MonoBehaviour
    {
        [Tooltip("List of positions where players can spawn")]
        [SerializeField] private Vector2[] spawnPositions;

        /// <summary>
        /// Spawns players into the scene with the given character and inventory information.
        /// </summary>
        /// <param name="character">The character to spawn.</param>
        public void SpawnPlayer(Character character) // Need a way to pass player's inventory
        {
        }
    }
}