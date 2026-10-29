#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Player
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class PlayerRegistery
    {
        /// <summary>
        /// The list of active players in game.
        /// </summary>
        public static Player[] Players;
        public static readonly int MaxPlayers = 2;

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
                    return i;
                }
            }
            return -1;
        }
    }
}