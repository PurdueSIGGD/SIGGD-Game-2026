using UnityEngine;

public abstract class ProjectileTool : Tool
{
    [SerializeField] protected ToolProjectileData ProjectileData;
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Transform ArmRotatePoint;

    RotateArmToAim aim;

    void Awake() => aim = ArmRotatePoint.GetComponent<RotateArmToAim>();

    protected ToolProjectile Fire()
    {
        Debug.Log("tool projectile fire called");
        if (ProjectileData == null)
        {
            Debug.Log("projectile data null");
        }

        if (FirePoint == null)
        {
            Debug.Log("fire point null");
        }
        if (ArmRotatePoint == null)
        {
            Debug.Log("arm rotate null");
        }

        return ProjectileManager.Instance.ShootToolProjectile(ProjectileData, FirePoint, aim.AimDirection);
    }

    public virtual void Equip(Transform newFirepoint, Transform newArmRotatePoint)
    {
        Debug.Log("equip happened");
        FirePoint = newFirepoint;
        ArmRotatePoint = newArmRotatePoint; // will be getting set to the same thing each time but needs to be set at least once
        aim = ArmRotatePoint.GetComponent<RotateArmToAim>();
    }

    //void OnDrawGizmos() { if (firePoint && aim)
    //Gizmos.DrawRay(firePoint.position, aim.AimDirection * 3f); }
}
