using UnityEngine;

public abstract class ProjectileTool : Tool
{
    [SerializeField] protected ToolProjectileData projectileData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform armRotatePoint;

    RotateArmToMouse aim;

    void Awake() => aim = armRotatePoint.GetComponent<RotateArmToMouse>();

    protected ToolProjectile Fire()
    {
        return ProjectileManager.Instance.ShootToolProjectile(projectileData, firePoint, aim.AimDirection);
    }

    //void OnDrawGizmos() { if (firePoint && aim)
    //Gizmos.DrawRay(firePoint.position, aim.AimDirection * 3f); }
}
