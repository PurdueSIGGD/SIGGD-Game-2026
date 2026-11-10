using UnityEngine;

public class BombThrower : ProjectileTool
{
    protected override void OnUse() => Fire();

    // leaving this here as a reminder that we can make custom equip methods for individual tools if the genral one isnt enough
    /*
    public override void Equip(Transform newFirepoint, Transform newArmRotatePoint)
    {
        base.Equip(newFirepoint, newArmRotatePoint);

        Debug.Log("bomb thrower equip");
    }
    */
}
