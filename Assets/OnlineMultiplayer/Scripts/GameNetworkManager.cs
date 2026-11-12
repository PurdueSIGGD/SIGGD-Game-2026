using UnityEngine;
using Steamworks;

public class GameNetworkManager : MonoBehaviour
{
    // game to connnect to
    public uint appId = 480;

    private void Awake()
    {
        // login through steam to startup the game
        DontDestroyOnLoad(this);
        try
        {
            SteamClient.Init(appId, true);
            Debug.Log("Steam is running");
        }
        catch (System.Exception e) 
        {
            Debug.Log(e.Message);
        }
    }

    // shutdown connection to steam
    private void OnDisable() 
    {
        try 
        {
            Steamworks.SteamClient.Shutdown();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
