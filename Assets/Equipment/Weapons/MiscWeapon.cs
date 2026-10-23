using UnityEngine;

[CreateAssetMenu(fileName = "MiscWeapon", menuName = "Scriptable Objects/MiscWeapon")]
public class MiscWeapon : ScriptableObject, IEquipment
{
    public string Name;
    public float Cooldown;
    public MiscellaneousScriptableObject MiscProjectile;

    public void Use()
    {

    }
}