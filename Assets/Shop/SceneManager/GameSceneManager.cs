using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : SerializedMonoBehaviour
{
    //odin inspector 
    public enum gameScenes {
        MainMenu,
        BossOne,
        BossTwo,
        BossThree,
        DeathScreen,
        ResultsScreen,
        Shop
    }

    //singleton code
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    [OdinSerialize] Dictionary<gameScenes, SceneAsset> sceneList = new Dictionary<gameScenes, SceneAsset>();



    public void LoadScene(gameScenes sceneType) 
    {
        if (sceneList.TryGetValue(sceneType, out SceneAsset scene))
        {
            SceneManager.LoadScene(scene.name);
        }
        else
        {
            Debug.LogError("Scene not found!");
        }
    }

    
    
}
