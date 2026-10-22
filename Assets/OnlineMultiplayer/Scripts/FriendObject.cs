using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class FriendObject : MonoBehaviour
{
    public SteamId steamid;

    public async void Invite()
    {
        if(SteamLobbyManager.UserInLobby) 
        {
            SteamLobbyManager.currentLobby.InviteFriend(steamid);
            Debug.Log("Invited " + steamid);
        }
        else
        {
            bool result = await SteamLobbyManager.CreateLobby();
            if (result) 
            {
                SteamLobbyManager.currentLobby.InviteFriend(steamid);
                Debug.Log("Invited " + steamid + " and created a new lobby");
            }
            // Debug.Log("Player is not in lobby, so did not send invite request");
        }
    }
}
