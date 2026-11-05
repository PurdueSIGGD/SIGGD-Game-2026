using UnityEngine;

[CreateAssetMenu(fileName = "TimedBombMiscWeapon", menuName = "Scriptable Objects/TimedBombMiscWeapon")]
public class TimedBombProjectile : BombProjectile
{
    public override void Detonate(MiscBullet bomb)
    {
        base.Detonate(bomb);
    }
}
