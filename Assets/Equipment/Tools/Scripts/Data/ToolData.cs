using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "Scriptable Objects/ToolData")]
public class ToolData : ScriptableObject
{
    [field: SerializeField, Tooltip("Name of the tool")]
    public string toolName { get; private set; }
    [field: SerializeField, Tooltip("Cooldown in seconds of the tool")]
    public float cooldown { get; private set; } = 0;
    [field: SerializeField, Tooltip("Tool item sprite")]
    public Sprite sprite { get; private set; }
}
