using UnityEngine;
using Extensions.Singleton;

public class ToolProjectileManager : Singleton<ToolProjectileManager>
{
    public ToolProjectile Spawn(ToolProjectileData data, Transform firePoint, Vector2 direction)
    {
        GameObject obj = Instantiate(data.prefab, firePoint.position, firePoint.rotation);
        ToolProjectile projectile = obj.GetComponent<ToolProjectile>();
        projectile.Init(data, direction.normalized);
        return projectile;
    }
}
