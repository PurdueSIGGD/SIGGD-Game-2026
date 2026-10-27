using UnityEngine;

[CreateAssetMenu(fileName = "BombThrowerMiscWeapon", menuName = "Scriptable Objects/BombThrowerMiscWeapon")]
public class BombThrowerMiscWeapon : MiscWeapon
{
    public override void Use()
    {
        Debug.Log("Fwhoosh!");
    }
}
