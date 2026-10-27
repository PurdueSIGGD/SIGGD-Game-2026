using UnityEngine;

[CreateAssetMenu(fileName = "BombThrowerMiscWeapon", menuName = "Scriptable Objects/BombThrowerMiscWeapon")]
public class BombThrowerMiscWeapon : MiscWeapon
{
    public override void Use(Transform from)
    {
        Debug.Log("Fwhoosh!");
        ProjectileManager.Instance.ShootMiscTowardsMouse(MiscProjectile, from);
    }
}
