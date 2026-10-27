using UnityEngine;

[CreateAssetMenu(fileName = "GrapplingHookMiscWeapon", menuName = "Scriptable Objects/GrapplingHookMiscWeapon")]
public class GrapplingHookMiscWeapon : MiscWeapon
{
    public override void Use()
    {
        Debug.Log("Shink!");
    }

    public void ReelIn()
    {
        Debug.Log("Reeling In!");
    }
}
