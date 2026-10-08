using UnityEngine;
using UnityEngine.Pool;
using Extensions.Singleton;

public class ProjectileManager : Singleton<ProjectileManager>
{

    public ObjectPool<GameObject> ProjectilePool;

    [SerializeField] private GameObject bulletPrefab;

    protected override void Awake()
    {
        base.Awake();

        // setting up actions for the projectilePool
        ProjectilePool = new ObjectPool<GameObject>(
            createFunc: MakeProjectile,
            actionOnRelease: OnRelease,
            actionOnGet: OnGet,
            actionOnDestroy: OnDestroyItem,
            defaultCapacity: 20, // allocates memory for the first 20 objects
            collectionCheck: true
        );
    }

    // will be called when a new projectile needs to be made
    private GameObject MakeProjectile()
    {
        // we dont have a way to access transform or rotation so we just 0 them for now and theyll get set when the bullet values are populated
        GameObject newBullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        newBullet.SetActive(false);
        return newBullet;
    }

    private void OnRelease(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }

    private void OnGet(GameObject pooledObject)
    {
        pooledObject.SetActive(true);
    }

    // if above max pool size or pool needs to destroy an object it will use this, might not be needed but good to have
    private void OnDestroyItem(GameObject pooledObject)
    {
        Destroy(pooledObject);
    }

    /// <summary>
    /// Shoots projectile towards the mouse, only needed to be called when a player is shooting a projectile
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    public void ShootProjectileTowardsMouse(ProjectileScriptableObject projectileScriptableObject, Transform firePoint)
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

        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, targetAngleZ, direction);
    }

    /// <summary>
    /// Shoots the projectile towards the passed in direction vector for boss attacks / enemy attacks
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="direction"> The direction that the projectile should be shot in </param>
    public void ShootProjectileTowardsDirection(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction) // this could not work i dunno testing will come later
    {
        // getting bullet objects
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // the target rotation that the bullet should get set to
        float targetAngleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, targetAngleZ, direction);
    }

    /// <summary>
    /// Shoots the projectile towards a target point
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="targetPoint"> the target that the projectile is being shot to </param>
    public void ShootProjectileTowardsPoint(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Transform targetPoint)
    {
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // make direction vector from the starting point and end point
        Vector2 direction = (targetPoint.position - firePoint.position).normalized;

        // the target rotation that the bullet should get set to
        float targetAngleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, targetAngleZ, direction);
    }
}
