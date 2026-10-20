using UnityEngine;

[CreateAssetMenu(fileName = "MiscObject", menuName = "Scriptable Objects/MiscObject")]
public class MiscObject : ScriptableObject
{
    public MiscellaneousScriptableObject MiscProjectile;
    public string Name;
    public float Cooldown;
}
