using UnityEngine;
using UnityEngine.Pool;
using Extensions.Singleton;

public class ProjectileManager : Singleton<ProjectileManager>
{

    private ObjectPool<GameObject> projectilePool;

    [SerializeField] private GameObject Bullet;

    protected override void Awake()
    {
        base.Awake();

        // setting up actions for the projectilPool
        projectilePool = new ObjectPool<GameObject>(
            createFunc: MakeProjectile
        );
    }

    // will be called when a new projectile needs to be made
    private GameObject MakeProjectile()
    {
        return null;
    }


    // what other scripts call to shoot projectile
    public void ShootProjectile(ProjectileScriptableObject projectileScriptableObject, Vector2 direction)
    {
        // make/get projectile from the pool then assign the values from projectile scriptable object onto it then shoot in direciton
        GameObject bullet = projectilePool.Get();

    }
}
