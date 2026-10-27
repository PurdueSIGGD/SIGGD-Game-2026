using UnityEngine;

[CreateAssetMenu(fileName = "MagnetMiscWeapon", menuName = "Scriptable Objects/MagnetMiscWeapon")]
public class MagnetMiscWeapon : MiscWeapon
{
    public override void Use(Transform from)
    {
        Debug.Log("Buzz...");
    }
}
