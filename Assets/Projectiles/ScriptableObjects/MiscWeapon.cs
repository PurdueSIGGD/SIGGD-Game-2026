using UnityEngine;

[CreateAssetMenu(fileName = "MiscObject", menuName = "Scriptable Objects/MiscObject")]
public class MiscWeapon: ScriptableObject
{
    public string Name;
    public float Cooldown;
    public MiscellaneousScriptableObject MiscProjectile;
}
