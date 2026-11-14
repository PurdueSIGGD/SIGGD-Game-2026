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

        Rigidbody2D bulletRigidbody = pooledObject.GetComponent<Rigidbody2D>();
        bulletRigidbody.linearVelocity = Vector3.zero;
        bulletRigidbody.angularVelocity = 0f;
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
    /// Shoots the projectile towards the passed in direction vector for boss attacks / enemy attacks
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="direction"> The direction that the projectile should be shot in </param>
    /// <param name="layerToHit"> The layers the projectile can hit </param>
    /// <param name="layerToIgnore"> The layers the projectile passes through </param>
    public void ShootProjectileTowardsDirection(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction, LayerMask layerToHit, LayerMask layerToIgnore) // this could not work i dunno testing will come later
    {
        ShootProjectile(projectileScriptableObject, firePoint, direction, layerToHit, layerToIgnore);
    }

    /// <summary>
    /// Shoots the projectile towards a target point
    /// </summary>
    /// <param name="projectileScriptableObject"> The scriptableobject that holds all the values for the bullet </param>
    /// <param name="firePoint"> The point where the bullet is shooting from </param>
    /// <param name="targetPoint"> the target that the projectile is being shot to </param>
    /// <param name="layerToHit"> The layers the projectile can hit </param>
    /// <param name="layerToIgnore"> The layers the projectile passes through </param>
    public void ShootProjectileTowardsPoint(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Transform targetPoint, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        // make direction vector from the starting point and end point
        Vector2 direction = (targetPoint.position - firePoint.position).normalized;

        ShootProjectile(projectileScriptableObject, firePoint, direction, layerToHit, layerToIgnore);
    }

    private void ShootProjectile(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction, LayerMask layerToHit, LayerMask layerToIgnore)
    {
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, direction.normalized, layerToHit, layerToIgnore);
    }
    #endregion

    /// <summary>
    /// Shoots a tool projectile along the arm from the fire point
    /// </summary>
    public ToolProjectile ShootToolProjectile(ToolProjectileData data, Transform firePoint, Vector2 direction)
    {
        Debug.Log("in shoot tool projectile");
        if (data == null) Debug.Log("projectile data null");
        if (firePoint == null) Debug.Log("fire point null");
        if (direction == null) Debug.Log("arm rotate null");

        GameObject obj = Instantiate(data.prefab, firePoint.position, firePoint.rotation);
        ToolProjectile projectile = obj.GetComponent<ToolProjectile>();
        projectile.Init(data, direction.normalized);
        return projectile;
    }
}
