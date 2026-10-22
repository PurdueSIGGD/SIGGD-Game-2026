using UnityEditor;

namespace Player
{
    [InitializeOnLoad]
    public static class PlayerRegistery
    {
        /// <summary>
        /// The list of active players in game.
        /// </summary>
        public static Player[] Players;
        public static readonly int MaxPlayers = 2;
        public static int PlayerCount;
        public static bool IsMultiplayer => PlayerCount > 1;

        static PlayerRegistery()
        {
            Players = new Player[MaxPlayers];
            PlayerCount = 0;
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
                PlayerCount = 0;
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
    }
}