using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryWeapon", menuName = "Scriptable Objects/PrimaryWeapon")]
public class PrimaryWeapon : ScriptableObject, IEquipment
{
    public ProjectileScriptableObject projectile;
    public string weaponName;
    public int numBullets;
    public float spread;
    public float reloadTime;
    public float offset;
    public float recoil;
    public int ammoCapacity;
    public float fireSpeed;

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