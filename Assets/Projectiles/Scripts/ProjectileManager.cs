using UnityEngine;
using UnityEngine.Pool;
using Extensions.Singleton;

public class ProjectileManager : Singleton<ProjectileManager>
{

    public ObjectPool<GameObject> ProjectilePool;

    [SerializeField] private GameObject Bullet;

    protected override void Awake()
    {
        base.Awake();

        // setting up actions for the projectilePool
        ProjectilePool = new ObjectPool<GameObject>(
            createFunc: MakeProjectile,
            actionOnRelease: OnRelease,
            collectionCheck: true
        );
    }

    // will be called when a new projectile needs to be made
    private GameObject MakeProjectile()
    {
        return null;
    }

    private void OnRelease(GameObject objectToRelease)
    {
        objectToRelease.SetActive(false);
    }


    // what other scripts call to shoot projectile
    public void ShootProjectile(ProjectileScriptableObject projectileScriptableObject, Transform firePoint) // direciton might have to be the mouse position as thats the direction it needs to shoot in but thatll mess up velocity stuff so we might need a normalized direction thing i dunno man
    {
        // getting bullet objects
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        
        // rotate spawned bullet to face mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        // the target rotation that the bullet shoult get set to
        float targetAngleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bulletScript.PopulateBulletValues(projectileScriptableObject, targetAngleZ, direction);
    }
}
