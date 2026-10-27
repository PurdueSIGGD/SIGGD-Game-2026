using UnityEngine;

public class MiscWeapon : ScriptableObject
{
    public MiscellaneousScriptableObject MiscProjectile;
    public float Cooldown;
    public string Type;
    
    public GameObject MiscWeaponVisualPrefab; // prefab for what the misc should look like when equipped
    public float MiscWeaponVisualRotationOffset; // for if model needs to be facing a different direction when spawned probably not needed but good to have

    public virtual void Use()
    {
        Debug.Log("Used!");
    }
}