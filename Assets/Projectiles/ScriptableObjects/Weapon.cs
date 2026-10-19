using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
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
}
/* this class creates a scriptable object for gun weapons! simply create a new scriptable object
 * in the editor and then fill in values to create your gun. PrimaryGun will use this scriptable object
 * and make the magic happen! - ryan :]
 */