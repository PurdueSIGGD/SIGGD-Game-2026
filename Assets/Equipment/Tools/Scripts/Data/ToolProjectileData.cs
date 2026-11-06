using UnityEngine;

[CreateAssetMenu(fileName = "ToolProjectileData", menuName = "Scriptable Objects/ToolProjectileData")]
public class ToolProjectileData : ScriptableObject
{
    public GameObject prefab;
    public float speed;
    public float lifetime;
}
