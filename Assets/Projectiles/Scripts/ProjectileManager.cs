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
            defaultCapacity: 20, // allocates the memory for the first 20 objects so that can be created quicker when they are called to be created
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

    // this spawns the projectile at the firepoint, rotates it to face the MOUSE and then shoots it with the velocity in the direction from the firepoint to the mouse
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

        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, targetAngleZ, direction);
    }

    // this overload is here because bosses wont want to shoot towards the mouse and instead will have a firepoint but also a target which could be the player or could be
    // this direction can be calculated by whoever is shooting the boss and direciton can be the difference between the player and the firepoint or can just be a point in the space that the boss is shooting towards
    public void ShootProjectile(ProjectileScriptableObject projectileScriptableObject, Transform firePoint, Vector2 direction)
    {
        // getting bullet objects
        GameObject bullet = ProjectilePool.Get();
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // the target rotation that the bullet shoult get set to
        float targetAngleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bulletScript.PopulateBulletValues(projectileScriptableObject, firePoint, targetAngleZ, direction);
    }
}
