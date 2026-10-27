using UnityEngine;

[CreateAssetMenu(fileName = "TimedBombMiscWeapon", menuName = "Scriptable Objects/TimedBombMiscWeapon")]
public class TimedBombProjectile : BombProjectile
{
    public override void Detonate()
    {
        Debug.Log("Kaboom! Explodes and extends " + Radius + " after " + Timer + " seconds.");
    }
}
