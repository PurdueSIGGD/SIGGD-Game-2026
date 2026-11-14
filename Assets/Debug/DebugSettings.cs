using UnityEngine;
[CreateAssetMenu(fileName = "Debug Settings", menuName = "Debug/Debug Settings")]
public class DebugSettings : ScriptableObject
{
    private static DebugSettings _instance;
    public static DebugSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<DebugSettings>("DebugSettings");
            return _instance;
        }
    }

#if UNITY_EDITOR
    public const bool IsEditor = true;
#else
    public const bool IsEditor = false;
#endif

    [SerializeField, Tooltip("Enables or disables all debug settings.")]
    private bool enableDebugMode;
    public bool EnableDebugMode => (enableDebugMode && IsEditor);

    [Header("Player Debug Settings")]
    
    [SerializeField, Tooltip("Player will not take damage when this is enabled.")]
    private bool enablePlayerInvincibility;
    [SerializeField, Tooltip("Player will not die, but can still be damaged, when this is enabled.")]
    private bool disablePlayerDeath;

    public bool EnablePlayerInvincibility => (enablePlayerInvincibility && EnableDebugMode);
    public bool DisablePlayerDeath => (disablePlayerDeath && EnableDebugMode);

}
