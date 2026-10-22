using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

public class FriendObject : MonoBehaviour
{
    public SteamId steamid;

    public void Invite()
    {
        try 
        {
            SteamLobbyManager.currentLobby.InviteFriend(steamid);
            Debug.Log("Invite sent to friend");
        }
        catch 
        {

        }
    }
}
