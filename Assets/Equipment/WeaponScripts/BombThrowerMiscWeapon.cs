using UnityEngine;

[CreateAssetMenu(fileName = "BombThrowerMiscWeapon", menuName = "Scriptable Objects/BombThrowerMiscWeapon")]
public class BombThrowerMiscWeapon : MiscWeapon
{
    public override void Use(GameObject miscProjectileObject)
    {
        Debug.Log("Fwhoosh!");
    }
}
