using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon : ScriptableObject
{
    // bullet scriptable object goes here

    public string weaponName;
    public int numBullets;
    public float spread;
    public float reloadTime;
    public float offset;
    public float recoil;
    public int ammoCapacity;

}
