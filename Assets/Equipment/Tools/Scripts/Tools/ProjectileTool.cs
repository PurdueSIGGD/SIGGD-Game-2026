using UnityEngine;

public abstract class ProjectileTool : Tool
{
    [SerializeField] protected ToolProjectileData projectileData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform armRotatePoint;

    protected ToolProjectile Fire()
    {
        Vector2 aimDir = firePoint.position = armRotatePoint.position;
        return ToolProjectileManager.Instance.Spawn(projectileData, firePoint, aimDir);
    }
}
