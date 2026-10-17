using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public uint appId; 

    public void Awake() 
    {
        DontDestroyOnLoad(this);
        try
        {
            Steamworks.SteamClient.Init(appId, true);
            Debug.Log("Steam is up and running!"); // can't debug.log?
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);

            throw;
        }
    }
    
    private void OnApplicationQuit() 
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
