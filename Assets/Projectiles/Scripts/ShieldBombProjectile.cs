using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBombMiscWeapon", menuName = "Scriptable Objects/ShieldBombMiscWeapon")]
public class ShieldBombProjectile : BombProjectile
{
    public float Durability;

    public override void Detonate()
    {
        Debug.Log("Explodes and extends " + Radius + " after " + Timer + " seconds, protecting you with " + Durability + " durability!");
        base.Detonate();
    }
}
