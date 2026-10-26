using UnityEngine;
using UnityEngine.Pool;
using Extensions.Singleton;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public ObjectPool<GameObject> ProjectilePool;
    public ObjectPool<GameObject> MiscProjectilePool; // projectile pool for misc objects since they use a different prefab with different scripts

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject miscBulletPrefab;

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

        MiscProjectilePool = new ObjectPool<GameObject>(
            createFunc: MakeMiscProjectile,
            actionOnRelease: OnRelease,
            actionOnGet: OnGet,
            actionOnDestroy: OnDestroyItem,
            defaultCapacity: 2, // there probably wont be more than 2 but we'll see
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

    private GameObject MakeMiscProjectile()
    {
        GameObject newBullet = Instantiate(miscBulletPrefab, Vector3.zero, Quaternion.identity);
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

    #region Player / Bosses shooting bullets
    /// <summary>
    /// Shoots projectile towards the mouse, only needed to be called when a player is shooting a projectile
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    public void ShootProjectileTowardsMouse(ProjectileScriptableObject projectileScriptableObject, Transform firePoint)
    {
        // rotate spawned bullet to face mouse
        // Get mouse position on the screen
        Vector2 mouseScreenPosition = InputManager.Instance.PointerScreenPosition;

        // calculate the correct Z distance from the camera to the player/arm
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToCamera));

        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        ShootProjectile(projectileScriptableObject, firePoint, direction);
    }

    /// <summary>
    /// Shoots the projectile towards the passed in direction vector for boss attacks / enemy attacks
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="direction"> The direction that the projectile should be shot in </param>
    public void ShootProjectileTowardsDirection(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction) // this could not work i dunno testing will come later
    {
        ShootProjectile(projectileScriptableObject, firePoint, direction);
    }

    /// <summary>
    /// Shoots the projectile towards a target point
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="targetPoint"> the target that the projectile is being shot to </param>
    public void ShootProjectileTowardsPoint(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Transform targetPoint)
    {
        // make direction vector from the starting point and end point
        Vector2 direction = (targetPoint.position - firePoint.position).normalized;

        ShootProjectile(projectileScriptableObject, firePoint, direction);
    }

    private void ShootProjectile(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction)
    {
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, direction.normalized);
    }
    #endregion

    #region Player Shooting Misc Projectiles
    public void ShootMiscTowardsMouse(MiscellaneousScriptableObject miscScriptableObject, Transform firePoint)
    {
        // rotate spawned bullet to face mouse
        // Get mouse position on the screen
        Vector2 mouseScreenPosition = InputManager.Instance.PointerScreenPosition;

        // calculate the correct Z distance from the camera to the player/arm
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, distanceToCamera));

        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        ShootMiscProjectile(miscScriptableObject, firePoint, direction);
    }

    private void ShootMiscProjectile(MiscellaneousScriptableObject miscScriptableObject, Transform firePoint, Vector2 direction)
    {
        GameObject miscObject = MiscProjectilePool.Get();
        MiscBullet miscScript = miscObject.GetComponent<MiscBullet>();
        miscScript.PopulateMiscValues(miscScriptableObject, firePoint, direction.normalized);
    }
    #endregion
}
