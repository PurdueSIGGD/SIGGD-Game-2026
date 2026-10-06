using UnityEngine;
using UnityEngine.Pool;
using Extensions.Singleton;

public class ProjectileManager : Singleton<ProjectileManager>
{

    private ObjectPool<GameObject> projectilePool;

    protected override void Awake()
    {
        base.Awake();

        // setting up actions for the projectilPool
        projectilePool = new ObjectPool<GameObject>(
            createFunc: SpawnProjectile

        );
    }

    private void Start()
    {
        
    }

    private GameObject SpawnProjectile()
    {
        return null;
    }


    // what other scripts call to shoot projectile
    public void ShootProjectile(ProjectileScriptableObject projectileScriptableObject)
    {
        Bullet bullet = projectileScriptableObject.Bullet.GetComponent<Bullet>();
    }
}
