using UnityEngine;

[CreateAssetMenu(fileName = "MiscWeapon", menuName = "Scriptable Objects/MiscWeapon")]
public class MiscWeapon : ScriptableObject, IEquipment
{
    public MiscellaneousScriptableObject MiscProjectile;
    public string Name;
    public float Cooldown;
    
    public GameObject MiscWeaponVisualPrefab; // prefab for what the misc should look like when equipped
    public float MiscWeaponVisualRotationOffset; // for if model needs to be facing a different direction when spawned probably not needed but good to have

    public void Use()
    {
        // i dont think this will get used i think we will add a serilized scriptable reference or whatever the fuck theyre called
    }
}