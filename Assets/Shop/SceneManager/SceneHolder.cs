using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneHolder", menuName = "Scriptable Objects/SceneHolder")]
public class SceneHolder : ScriptableObject
{
    public string name;
    public SceneAsset scene;
}
