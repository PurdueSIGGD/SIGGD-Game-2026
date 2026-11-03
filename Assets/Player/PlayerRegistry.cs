namespace PlayerSystem
{
    public static class PlayerRegistry
    {
        /// <summary>
        /// The list of active players in game.
        /// </summary>
        public static Player[] Players { get; private set; }
        public const int MaxPlayers = 2;
        public static int PlayerCount;
        public static bool IsMultiplayer => PlayerCount > 1;

        static PlayerRegistry()
        {
            Players = new Player[MaxPlayers];
            PlayerCount = 0;
        }

        /// <summary>
        /// Registers the player as an active player for easy look up.
        /// </summary>
        /// <returns> the index of the player, returns -1 if exceeds max player count </returns>
        public static int RegisterPlayer(Player player)
        {
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

        /// <summary>
        /// Removes the player from the active players, freeing its slot.
        /// </summary>
        public static void UnregisterPlayer(Player player)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == player)
                {
                    Players[i] = null;
                    return;
                }
            }
        }
    }
}