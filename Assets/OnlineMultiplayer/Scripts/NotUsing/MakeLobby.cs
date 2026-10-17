using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class MakeLobby : MonoBehaviour
{
    Lobby myLobby;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);
        SetupCallbacks();
    }

    void SetupCallbacks() 
    {
        // add callback functions. when an event is triggered, run whats in the function, or else nothing will happen when event is triggered
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreate; // To run functions call them here
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
    }

    /////////////////////////////////////// callback functions
    void OnLobbyCreate(Result res, Lobby lobby)
    {
        try
        {
            if (!myLobby.Id.IsValid)
            {
                myLobby = lobby;
                Debug.Log("Created lobby");

                // send invite FOR TESTING ONLY - IDEALLY ON BUTTON PRESS
                testInviteFriend();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("error in making lobby");
            Debug.Log(e.Message);
        }
    }

    void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log(friend.Name + " is joining the lobby");
    }
    /////////////////////////////////////////////

    async void Start()
    {
        await SteamMatchmaking.CreateLobbyAsync(2); // this number is how many players can join the lobby
        // cannot put any code below that relies on lobby being created since the callback is not instantly co-lled when creatLobby() finishes
        // put in onlobbycreate callback instead
    }

    void testInviteFriend() {
        foreach (var player in SteamFriends.GetFriends())
        {
            //Debug.Log(player.Name);
            if (player.Name == "sspbj") // this is the invited player's name
            {
                // might not work if game is closed
                myLobby.SetJoinable(true);
                myLobby.SetPublic();
                //send lobby id as string so they can connect
                Debug.Log(myLobby.Id.ToString());
                player.InviteToGame(myLobby.Id.ToString()); // sends invite link via steam dms
                // SteamFriends.OpenGameInviteOverlay(myLobby.Id);
                // myLobby.InviteFriend(friendToInvite.Id);
                // SteamFriends a = player;
                // Debug.Log("invite sent");
                // break;
            }
        }
    }


}
