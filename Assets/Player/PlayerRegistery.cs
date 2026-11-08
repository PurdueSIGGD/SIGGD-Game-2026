using UnityEditor;
using UnityEngine;

namespace PlayerSystem
{
    [InitializeOnLoad]
    public static class PlayerRegistery
    {
        /// <summary>
        /// The list of active players in game.
        /// </summary>
        public static Player[] Players;
        public static readonly int MaxPlayers = 2;
        public static int PlayerCount { get; private set; } = 0;

        static PlayerRegistery()
        {
            Players = new Player[MaxPlayers];
        }

        /// <summary>
        /// Registers the player as an acitve player for easy look up.
        /// </summary>
        /// <returns> the index of the player, returns -1 if exceeds max player count </returns>
        public static int RegisterPlayer(Player player)
        {
            if (Players.Length == 0)
            {
                Players = new Player[MaxPlayers];
            }

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null)
                {
                    Players[i] = player;
                    PlayerCount++;
                    return i;
                }
            }
            return -1;
        }

        public static void UnregisterPlayer(Player player)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player)
                {
                    Players[i] = null;
                    PlayerCount--;
                    return;
                }
            }
        }

        public static Player GetRandomPlayer()
        {
            if (PlayerCount == 0)
            {
                return null;
            }
            return Players[Random.Range(0, PlayerCount)];
        }
    }
}