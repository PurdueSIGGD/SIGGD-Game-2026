using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public enum gameScenes {
        MainMenu,
        BossOne,
        BossTwo,
        BossThree,
        DeathScreen,
        ResultsScreen,
        Shop
    }


    [Serrializable] Dictionary<gameScenes, SceneAsset> sceneList = new Dictionary<gameScenes, SceneAsset>();



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
