using UnityEngine;

[CreateAssetMenu(fileName = "RemoteBombMiscWeapon", menuName = "Scriptable Objects/RemoteBombMiscWeapon")]
public class RemoteBombProjectile : BombProjectile
{
    public override void Detonate(MiscBullet bomb)
    {
        Debug.Log("Explodes and extends " + Radius + " after " + Timer + " seconds, or after a remote activation.");
        base.Detonate(bomb);
    }
}
