using UnityEngine;

[CreateAssetMenu(fileName = "TimedBombMiscWeapon", menuName = "Scriptable Objects/TimedBombMiscWeapon")]
public class TimedBombProjectile : BombProjectile
{
    public override void Detonate(MiscBullet bomb)
    {
        Debug.Log("Explodes and extends " + Radius + " after " + Timer + " seconds.");
        base.Detonate(bomb);
    }
}
