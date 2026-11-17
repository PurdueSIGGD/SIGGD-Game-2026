using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Pause execution for the specified number of seconds
        yield return new WaitForSeconds(5);

        // Load the target scene
        GameSceneManager.Instance.LoadScene(GameSceneManager.gameScenes.BossOne);
    }
}
