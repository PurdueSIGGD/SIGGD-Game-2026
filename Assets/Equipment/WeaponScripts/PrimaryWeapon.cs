using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryWeapon", menuName = "Scriptable Objects/PrimaryWeapon")]
public class PrimaryWeapon : ScriptableObject, IEquipment
{
    public ProjectileScriptableObject Projectile;
    public string WeaponName;
    public int NumBullets;
    public float Spread;
    public float ReloadTime;
    public float Offset;
    public float Recoil;
    public int AmmoCapacity;
    public float FireSpeed;

    public GameObject WeaponVisualPrefab; // prefab for what the gun should look like when equipped
    public float WeaponVisualRotationOffset; // for if model needs to be facing a different direction when spawned probably not needed but good to have

    public void Use()
    {

    }

}
/* this class creates a scriptable object for gun weapons! simply create a new scriptable object
 * in the editor and then fill in values to create your gun. PrimaryGun will use this scriptable object
 * and make the magic happen! - ryan :]
 */