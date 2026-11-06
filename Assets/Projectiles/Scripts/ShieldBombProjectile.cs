using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBombMiscWeapon", menuName = "Scriptable Objects/ShieldBombMiscWeapon")]
public class ShieldBombProjectile : BombProjectile
{
    public float Durability;

    public override void Detonate(MiscBullet bomb)
    {
        Debug.Log("Protecting you with " + Durability + " durability!");
        base.Detonate(bomb);
    }
}
