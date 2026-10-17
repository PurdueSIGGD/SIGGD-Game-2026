using UnityEngine;
using Steamworks;
using Steamworks.Data;

public class ReadyToJoin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);
        SetupCallbacks();
    }

    void SetupCallbacks()
    {
        // add callback functions. when an event is triggered, run whats in the function, or else nothing will happen when event is triggered

        SteamFriends.OnGameLobbyJoinRequested += OnGameInviteAccepted; // accept invite when game is closed
        SteamFriends.OnGameRichPresenceJoinRequested += OnGameRichPresenceJoinRequested; // accpet invite when game is open
    }

    async void OnGameInviteAccepted(Lobby lobby, SteamId steamId)
    {
        Debug.Log("OnGameInviteAccepted called, trying to join lobby");
        await lobby.Join();
    }

    async void OnGameRichPresenceJoinRequested(Friend friend, string s)
    {
        Debug.Log("OnGameRichPresenceJoinRequested called, trying to join lobby");
        Debug.Log("the string is: " + s);
        if (ulong.TryParse(s, out ulong seshId))
        {
            Debug.Log("seshID is valid");
            Lobby? lob = await SteamMatchmaking.JoinLobbyAsync(seshId);
            if(lob == null)
            {
                Debug.Log("ERROR, LOBBY TRIED TO JOIN IS NULL!");
            }
        }
        else 
        {
            Debug.Log("bad lobby sesh id");
        }
    }
}
