using UnityEngine;

[CreateAssetMenu(fileName = "RemoteBombMiscWeapon", menuName = "Scriptable Objects/RemoteBombMiscWeapon")]
public class RemoteBombProjectile : BombProjectile
{
    public override void Detonate(MiscBullet bomb)
    {
        Debug.Log("Explodes after a remote activation or after a long enough time passes.");
        base.Detonate(bomb);
    }
}
