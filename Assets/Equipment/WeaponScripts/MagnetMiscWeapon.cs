using UnityEngine;

[CreateAssetMenu(fileName = "MagnetMiscWeapon", menuName = "Scriptable Objects/MagnetMiscWeapon")]
public class MagnetMiscWeapon : MiscWeapon
{
    public override void Use(GameObject miscProjectileObject)
    {
        Debug.Log("Buzz...");
    }
}
